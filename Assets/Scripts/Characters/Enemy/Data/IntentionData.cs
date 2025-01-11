using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters.Enemy.Data
{
    public class IntentionData : SerializedScriptableObject
    {
        public Dictionary<string, Intention> Intentions;


        public Intention GetIntention(string id, string whoFinding = "")
        {
            if (Intentions.TryGetValue(id, out var intention))
            {
                return intention;
            }
            else
            {
                Debug.LogError($"{whoFinding} can't find Intention with ID '{id}' in IntentionData!");
                return null;
            }
        }
    }

    [Serializable]
    public class Intention
    {
        [SerializeField] private bool showIntentionValue;
        
        [SerializeField] private Sprite intentionSprite;
        [SerializeField] private string header;
        [SerializeField] private string content;
        [SerializeField] private ActionType _actionType;


        public Sprite IntentionSprite => intentionSprite;
        public string Header => header;
        public string Content => content;
        public bool ShowIntentionValue => showIntentionValue;
        public ActionType ActionType => _actionType;

    }

    [Flags]
    public enum ActionType
    {
        None = 0,
        Attack = 2,
        AddBuff = 4
    }
}