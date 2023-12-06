using UnityEngine;
using UnityEngine.UI;

namespace Ayush
{
    public class GameLoopController : LocalService
    {
        [SerializeField] private GameplayController _gameplayController;

        [SerializeField] private RuleSet _ruleSet;

        [SerializeField] private float _timeToSelectCard = 2f;

        [SerializeField] private Image _timerFillImage;

        private float _timeCounter = 0f;
        private bool _hasCpuSelectedAnyCard = false;
        private bool _hasPlayerSelectedAnyCard = false;
        private MoveType _currentSelectedCpuCard;
        private MoveType _currentSelectedPlayerCard;
        private int _currentWinStreak = 0;
        public bool CanSelectCard => (_timeCounter <= _timeToSelectCard && CurrentGameState == GameState.CardSelection);
        public GameState CurrentGameState { get; private set; }

        protected override void OnInit(bool serviceAdded)
        {
            base.OnInit(serviceAdded);
            _gameplayController.PlayerCardSelected += OnPlayerCardSelected;
            _gameplayController.RestartGame += OnRestartGame;
        }

        void Update()
        {
            if (CurrentGameState == GameState.Result)
                return;

            if (!_hasCpuSelectedAnyCard)
            {
                var allCards = _gameplayController.HandElementCardDatabase.MoveTypesArray;
                var length = allCards.Length;
                _currentSelectedCpuCard = allCards[Random.Range(0, length)];
                _hasCpuSelectedAnyCard = true;
            }

            if (CanSelectCard)
            {
                _timeCounter += Time.deltaTime;
                _timerFillImage.fillAmount = _timeCounter / _timeToSelectCard;
            }
            else
            {
                SetResult();
            }

            if (_hasPlayerSelectedAnyCard && _hasCpuSelectedAnyCard)
            {
                SetResult();
            }
        }

        protected override void OnDisposed(bool serviceRemoved)
        {
            if (_gameplayController == null)
                return;
            _gameplayController.PlayerCardSelected -= OnPlayerCardSelected;
            _gameplayController.RestartGame -= OnRestartGame;

            base.OnDisposed(serviceRemoved);
        }

        private void OnPlayerCardSelected(MoveType moveType)
        {
            if (CanSelectCard && !_hasPlayerSelectedAnyCard)
            {
                _hasPlayerSelectedAnyCard = true;
                _currentSelectedPlayerCard = moveType;
                _gameplayController.SetPlayerCardText(_currentSelectedPlayerCard.ToString());
            }
        }

        private void SetResult()
        {
            CurrentGameState = GameState.Result;
            _gameplayController.SetCpuCardText(_currentSelectedCpuCard.ToString());
            bool didPlayerWon = false;
            string resultString;

            if (_hasPlayerSelectedAnyCard)
            {
                var result = _ruleSet.GetGameResult(_currentSelectedPlayerCard, _currentSelectedCpuCard);
                didPlayerWon = _currentSelectedPlayerCard == result.Winner;
                var color = (didPlayerWon) ? "green" : "red";
                resultString = string.Format("<color={0}>{1} {2} {3}</color>", color, result.Winner, result.ResultText,
                    result.Loser);
            }
            else
            {
                resultString = "You Lost, You didn't choose any card";
            }

            _gameplayController.SetResultSequence(resultString, didPlayerWon);

            if (didPlayerWon)
            {
                _currentWinStreak++;
                _gameplayController.UpdateCurrentStreakText(_currentWinStreak);
            }
        }

        private void OnRestartGame()
        {
            if (CurrentGameState == GameState.Result)
            {
                _hasPlayerSelectedAnyCard = false;
                _hasCpuSelectedAnyCard = false;
                _timeCounter = 0;
                CurrentGameState = GameState.CardSelection;
            }
        }
    }

    public enum GameState
    {
        CardSelection,
        Result
    }
}