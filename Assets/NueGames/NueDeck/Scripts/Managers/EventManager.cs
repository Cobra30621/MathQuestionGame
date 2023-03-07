using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Managers
{
    public class EventManager : MonoBehaviour
    {
        public EventManager(){}
        public static EventManager Instance { get; private set; }
        
        public Action onAttacked;
        public Action<PowerType> OnPowerApplied;
        public Action<PowerType> OnPowerCleared;

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

