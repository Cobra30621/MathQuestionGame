using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class EventManager : MonoBehaviour
    {
        public EventManager(){}
        public static EventManager Instance { get; private set; }
        
        public Action<DamageInfo, int> onAttacked;
        // public Action<PowerType, int> OnPowerApplied;
        // public Action<PowerType, int> OnPowerChanged;
        // public Action<PowerType> OnPowerCleared;
        public Action<int> OnQuestioningModeEnd;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

}

