using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Managers
{
    /// <summary>
    /// 負責遊戲事件(被攻擊、答對題目、勝利等等)的觸發、訂閱
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        public EventManager(){}
        public static EventManager Instance { get; private set; }
        
        /// <summary>
        /// 事件：受到攻擊
        /// </summary>
        public Action<DamageInfo, int> onAttacked;
        // public Action<PowerType, int> OnPowerApplied;
        // public Action<PowerType, int> OnPowerChanged;
        // public Action<PowerType> OnPowerCleared;

        /// <summary>
        /// 事件: 答題
        /// </summary>
        public System.Action OnAnswer;
        /// <summary>
        /// 事件: 答對題目
        /// </summary>
        public System.Action OnAnswerCorrect;
        /// <summary>
        /// 事件: 答錯題目
        /// </summary>
        public System.Action OnAnswerWrong;
        /// <summary>
        /// 事件: 結束答題模式
        /// </summary>
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

