﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy.Data
{
    public class IntentionData : SerializedScriptableObject
    {
        public Dictionary<string, Intention> Intentions;


        public Intention GetIntention(string id)
        {
            if (Intentions.TryGetValue(id, out var intention))
            {
                return intention;
            }
            else
            {
                Debug.LogError($"Intention with ID '{id}' not found in IntentionData!");
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


        public Sprite IntentionSprite => intentionSprite;
        public string Header => header;
        public string Content => content;
        public bool ShowIntentionValue => showIntentionValue;

    }
}