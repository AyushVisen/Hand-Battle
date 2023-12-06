using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ayush
{
    [CreateAssetMenu(fileName = "Hand Element Card Database", menuName = "Ayush/HandElementCardDatabase")]
    public class HandElementCardDatabase : ScriptableObject
    {
        [SerializeField] private bool _reconstructDatabse = false;

        [field: SerializeField]
        public SerializedDictionary<MoveType, HandElementDetail> HandElementDetails { get; private set; }

        [field: SerializeField, ReadOnly] public MoveType[] MoveTypesArray { get; private set; }

        private void OnValidate()
        {
            if (_reconstructDatabse)
            {
                _reconstructDatabse = false;
                ReconstructDatabase();
            }
        }

        private void ReconstructDatabase()
        {
            MoveTypesArray = (MoveType[])(Enum.GetValues(typeof(MoveType)));
            foreach (var moveType in MoveTypesArray)
            {
                HandElementDetails.TryAdd(moveType, new HandElementDetail());
            }
        }
    }

    [Serializable]
    public class HandElementDetail
    {
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public AudioClip SelectAudioClip { get; private set; }

        [field: SerializeField] public Color CardBgColor { get; private set; }
    }
}