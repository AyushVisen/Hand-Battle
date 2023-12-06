using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ayush
{
    public class GameplayController : LocalService
    {
        [SerializeField] private AudioClip _bgmClip;

        [SerializeField] private AudioClip _winClip;

        [SerializeField] private AudioClip _loseClip;

        [SerializeField] private Transform _cardSpawnTransform;

        [SerializeField] private HandElementCard _cardPrefab;

        [field: SerializeField] public HandElementCardDatabase HandElementCardDatabase { get; private set; }

        [SerializeField] private GameObject _restartGameButton;

        [SerializeField] private GameObject _menuButton;

        [SerializeField] private TextMeshProUGUI _highScoreText;

        [SerializeField] private TextMeshProUGUI _currentStreakText;

        [SerializeField] private TextMeshProUGUI _resultText;

        [SerializeField] private TextMeshProUGUI _cpuSelectedCardText;

        [SerializeField] private TextMeshProUGUI _playerSelectedCardText;

        public event Action<MoveType> PlayerCardSelected;
        public event Action RestartGame;

        private PlayerDataService _playerDataService;
        private AudioService _audioService;

        protected override void OnInit(bool serviceAdded)
        {
            base.OnInit(serviceAdded);

            GameManager.TryGetService(out _playerDataService);
            GameManager.TryGetService(out _audioService);

            _highScoreText.SetText(_playerDataService.PlayerData.WinningStreak.ToString());
            _currentStreakText.SetText("0");

            _audioService.PlayBGM(true, _bgmClip);

            foreach (var (key, value) in HandElementCardDatabase.HandElementDetails)
            {
                var card = Instantiate(_cardPrefab, _cardSpawnTransform);
                card.Init(key, value, _audioService, OnCardClicked);
            }
        }

        private void OnCardClicked(MoveType moveType)
        {
            PlayerCardSelected?.Invoke(moveType);
        }

        protected override void OnDisposed(bool serviceRemoved)
        {
            PlayerCardSelected = null;
            base.OnDisposed(serviceRemoved);
        }

        public void SetPlayerCardText(string playerCardText)
        {
            _playerSelectedCardText.SetText(playerCardText);
            _playerSelectedCardText.rectTransform.DOPunchScale(Vector3.one * 0.4f, 1f);
        }

        public void SetCpuCardText(string cpuCardText)
        {
            _cpuSelectedCardText.SetText(cpuCardText);
            _cpuSelectedCardText.rectTransform.DOPunchScale(Vector3.one * 0.4f, 1f);
        }

        public void SetResultSequence(string resultString, bool didPlayerWon)
        {
            _resultText.rectTransform.parent.localPosition = new Vector3((didPlayerWon ? 2 : -2) * Screen.width, 0, 0);
            _resultText.rectTransform.parent.gameObject.SetActive(true);
            _resultText.SetText(resultString);
            _resultText.rectTransform.parent.DOLocalMoveX(0, 0.75f).OnComplete(() =>
            {
                _audioService.PlaySfx(didPlayerWon ? _winClip : _loseClip);
                SetAfterResultButton(didPlayerWon);
            }).SetEase(Ease.InOutBack);
        }

        public void SetAfterResultButton(bool didPlayerWon)
        {
            if (didPlayerWon)
            {
                _restartGameButton.SetActive(true);
            }
            else
            {
                _menuButton.SetActive(true);
            }
        }

        public void OnRestartClicked()
        {
            _restartGameButton.SetActive(false);
            _menuButton.SetActive(false);
            _cpuSelectedCardText.SetText("");
            _playerSelectedCardText.SetText("");
            _resultText.SetText("");
            _resultText.rectTransform.parent.gameObject.SetActive(false);
            RestartGame?.Invoke();
        }

        public void OnMenuClick()
        {
            SceneManager.LoadScene(Constants.MenuScene);
        }

        public void UpdateCurrentStreakText(int currentStreak)
        {
            _currentStreakText.SetText(currentStreak.ToString());

            if (currentStreak > _playerDataService.PlayerData.WinningStreak)
            {
                _playerDataService.PlayerData.WinningStreak = currentStreak;
                _highScoreText.SetText(currentStreak.ToString());
            }
        }
    }
}