using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NueGames.Action;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Managers
{
    /// <summary>
    /// 負責執行遊戲行為（GameAction）
    /// 提供 AddToTop, AddToBottom 方法，讓其他人加入想要執行的（GameAction）
    /// </summary>
    public class GameActionExecutor : MonoBehaviour
    {
        /// <summary>
        /// "待執行的遊戲行為清單"
        /// </summary>
        private List<GameActionBase> actions;
        /// <summary>
        /// 正在執行的遊戲行為
        /// </summary>
        private GameActionBase currentAction;
        /// <summary>
        /// 前一個遊戲行為
        /// </summary>
        private GameActionBase previousAction;

        /// <summary>
        /// 階段
        /// </summary>
        private Phase phase;
        /// <summary>
        /// 遊戲行為是否執行完成
        /// </summary>
        [SerializeField]private bool actionIsDone;
        
     
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
            actions = new List<GameActionBase>();
            phase = Phase.WaitingOnUser;
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
            switch (phase)
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
            if (actionIsDone && currentAction != null)
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
            currentAction.DoAction();
            Debug.Log($"Do action {currentAction.ToString()}");
            actionIsDone = false;
            StartCoroutine(DoActionRoutine(currentAction.Duration));
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
            previousAction = currentAction;
            currentAction = null;
            TryGetNextAction();
            if (currentAction == null ) {
                phase = Phase.WaitingOnUser;
            }
        }
        
        /// <summary>
        /// 嘗試取得下一個遊戲行為
        /// </summary>
        private void TryGetNextAction()
        {
            if (actions.Any() ) {
                currentAction = actions[0];
                actions.RemoveAt(0);
                phase = Phase.ExecutingAction;
            }
        }

        #endregion

        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲行為清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="cardActionDatas"></param>
        public void AddToBottom(List<GameActionBase> cardActionDatas)
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
        public void AddToTop(GameActionBase action) {
            actions.Insert(0, action);
        }
        
        /// <summary>
        /// 將想執行的遊戲行為，加到"待執行的遊戲行為清單"
        /// 排隊：會等原本的"待執行的遊戲行為清單"遊戲行為執行完，才執行
        /// </summary>
        /// <param name="action"></param>
        public void AddToBottom(GameActionBase action) {
            actions.Add(action);
        }
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