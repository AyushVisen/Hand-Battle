using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Ayush
{
    [CreateAssetMenu(fileName = "RuleSet", menuName = "Ayush/RuleSetAsset")]
    public class RuleSet : ScriptableObject
    {
        [FormerlySerializedAs("InitRulesSetup")]
        [Header(
            "Setup Tool, Just check the Field to do the desired action as the name suggests. These are like buttons, They will set themselves off as soon as you click on them.")]
        [SerializeField,
         Tooltip(
             "This will auto populate the Rules entries. Also it will preserve your previous entered values, In case a new MoveType is added")]
        private bool _initRulesSetup;

        [SerializeField, Tooltip("Click it only if you want fresh start.")]
        private bool _clearRulesSetup;

        [Header(
            "Game Rules Entries, You only have to set Winner/Loser/Result text for the below combination of matches")]
        [SerializeField, ReadOnly]
        private string _rulesValidityMessage;

        // SerializedDictionary needs "using UnityEngine.Rendering;" library
        public SerializedDictionary<MovePair, Result> GameRules;

        private bool RulesValidityCheck;

        private void OnValidate()
        {
            if (_clearRulesSetup)
            {
                ClearRules();
                _clearRulesSetup = false;
                return;
            }

            if (_initRulesSetup)
            {
                _initRulesSetup = false;
            }

            InitRules();
        }

        public Result GetGameResult(MoveType moveType1, MoveType moveType2)
        {
            var firstInput = (MoveType)Mathf.Min((int)moveType1, (int)moveType2);
            var secondInput = (MoveType)Mathf.Max((int)moveType1, (int)moveType2);

            return GameRules[new MovePair(firstInput, secondInput)];
        }

        [ContextMenu("Init Rules")]
        public void InitRules()
        {
            RulesValidityCheck = true;
            var moveTypesList = (MoveType[])(Enum.GetValues(typeof(MoveType)));

            for (int i = 0; i < moveTypesList.Length; i++)
            {
                for (int j = i; j < moveTypesList.Length; j++)
                {
                    var key = new MovePair(moveTypesList[i], moveTypesList[j]);

                    GameRules.TryAdd(key, new Result());

                    var result = GameRules[key];

                    if (moveTypesList[i] == moveTypesList[j])
                    {
                        result.Winner = result.Loser = moveTypesList[i];
                        result.ResultText = "Tie";
                    }

                    var isResultTextSetup = !string.IsNullOrEmpty(GameRules[key].ResultText);
                    var isWinnerLoserSetup = (result.Winner == moveTypesList[i] || result.Winner == moveTypesList[j])
                                             &&
                                             (result.Loser == moveTypesList[i] || result.Loser == moveTypesList[j])
                                             &&
                                             ((moveTypesList[i] == moveTypesList[j])
                                                 ? result.Winner == result.Loser
                                                 : result.Winner != result.Loser);

                    result.IsSetUp = isResultTextSetup && isWinnerLoserSetup;

                    RulesValidityCheck = RulesValidityCheck && result.IsSetUp;

                    if (!result.IsSetUp)
                    {
                        Debug.LogError($"Fix: ({moveTypesList[i]}, {moveTypesList[j]})");
                    }

                    GameRules[key] = result;
                }
            }

            _rulesValidityMessage = (RulesValidityCheck)
                ? "All rules now seems valid and logical".ToUpper()
                : "There are invalid Rules, Please fix them to Cause any error in Gameplay".ToUpper();
        }

        [ContextMenu("Clear Rules")]
        public void ClearRules()
        {
            GameRules.Clear();
        }
    }

    [Serializable]
    public struct MovePair
    {
        [ReadOnly] public MoveType Move1;

        [ReadOnly] public MoveType Move2;

        public MovePair(MoveType move1, MoveType move2)
        {
            Move1 = move1;
            Move2 = move2;
        }
    }

    [Serializable]
    public struct Result
    {
        [ReadOnly] public bool IsSetUp;
        public MoveType Winner;
        public string ResultText;
        public MoveType Loser;
    }

    public enum MoveType
    {
        Rock = 0,
        Paper,
        Scissors,
        Spock,
        Lizard
    }
}