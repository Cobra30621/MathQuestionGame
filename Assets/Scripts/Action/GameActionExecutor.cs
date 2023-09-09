using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Action.Parameters;
using NueGames.Action;
using NueGames.Parameters;
using NueGames.Power;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace NueGames.Managers
{
    /// <summary>
    /// 負責執行遊戲行為（GameAction）
    /// 提供 AddToTop, AddToBottom 方法，讓其他人加入想要執行的（GameAction）
    /// </summary>
    public class GameActionExecutor : SerializedMonoBehaviour
    {
        /// <summary>
        /// "待執行的遊戲行為清單"
        /// </summary>
        [OdinSerialize] private List<GameActionBase> _actions;
        /// <summary>
        /// 正在執行的遊戲行為
        /// </summary>
        [OdinSerialize] private GameActionBase _currentAction;
        /// <summary>
        /// 前一個遊戲行為
        /// </summary>
        [OdinSerialize] private GameActionBase _previousAction;

        /// <summary>
        /// 階段
        /// </summary>
        [ReadOnly] [OdinSerialize] private Phase _phase;
        /// <summary>
        /// 遊戲行為是否執行完成
        /// </summary>
        [ReadOnly] [OdinSerialize] private bool actionIsDone;
        
     
        /// <summary>
        /// 單例模式
        /// </summary>
        public static GameActionExecutor Instance { get; private set; }
        

        #region SetUp (初始化)
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

            Initialize();
        }

        void Initialize()
        {
            _actions = new List<GameActionBase>();
            _phase = Phase.WaitingOnUser;
            actionIsDone = true;
        }

        #endregion

        void Update()
        {
            HandleGameActions();
        }

        #region Action Coroutine
        /// <summary>
        /// 處理遊戲行為執行
        /// </summary>
        private void HandleGameActions()
        {
            switch (_phase)
            {
                case Phase.WaitingOnUser:
                    HandleWaitingOnUser();
                    break;
                case Phase.ExecutingAction:
                    HandleExecutingAction();
                    break;
            }
        }

        /// <summary>
        /// 處理：等待玩家階段
        /// </summary>
        private void HandleWaitingOnUser()
        {
            TryGetNextAction();
        }

        /// <summary>
        /// 處理：執行遊戲行為階段
        /// </summary>
        private void HandleExecutingAction()
        {
            if (actionIsDone && _currentAction != null)
            {
                DoCurrentAction();
            }
            else
            {
                GetNextAction();
            }
        }
        
        /// <summary>
        /// 執行現在的遊戲行為
        /// </summary>
        private void DoCurrentAction()
        {
            actionIsDone = false;
            try
            {
                Debug.Log($"Do action {_currentAction.GetType()} : {_currentAction}");
                _currentAction.DoAction();
            }
            catch (Exception e)
            {
                Debug.LogError($"Do Action {_currentAction.GetType()} Fail.\n Error {e}");
            }
            
            StartCoroutine(DoActionRoutine(_currentAction.ActionDelay));
        }

        /// <summary>
        /// TODO 註解
        /// </summary>
        /// <param name="wait"></param>
        /// <returns></returns>
        private IEnumerator DoActionRoutine(float wait)
        {
            yield return new WaitForSeconds(wait);
            actionIsDone = true;
        }
        
        /// <summary>
        /// 取得下一個遊戲行為
        /// </summary>
        private void GetNextAction()
        {
            _previousAction = _currentAction;
            _currentAction = null;
            TryGetNextAction();
            if (_currentAction == null ) {
                _phase = Phase.WaitingOnUser;
            }
        }
        
        /// <summary>
        /// 嘗試取得下一個遊戲行為
        /// </summary>
        private void TryGetNextAction()
        {
            if (_actions.Any() ) {
                _currentAction = _actions[0];
                _actions.RemoveAt(0);
                _phase = Phase.ExecutingAction;
            }
        }

        #endregion

        #region 將遊戲行為加入執行緒

        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲行為清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="cardActionDatas"></param>
        public static void AddToBottom(List<GameActionBase> cardActionDatas)
        {
            foreach (var playerAction in cardActionDatas)
            {
                AddToBottom(playerAction);
            }
        }
        
        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲行為清單"
        /// 插隊：優先執行
        /// </summary>
        /// <param name="action"></param>
        public static void AddToTop(GameActionBase action) {
            Instance._actions.Insert(0, action);
        }

        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲行為清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="action"></param>
        public static void AddToBottom(GameActionBase action)
        {
            Instance._actions.Add(action);
        }
        
        
        

        #endregion

    }

    /// <summary>
    /// 階段
    /// </summary>
    public enum Phase
    {
        WaitingOnUser, // 等待玩家
        ExecutingAction // 正在執行遊戲行為
    }
}