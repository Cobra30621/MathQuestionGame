using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NueGames.Action;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Managers
{
    public class GameActionExecutor : MonoBehaviour
    {
        private List<GameActionBase> actions;
        private GameActionBase currentAction;
        private GameActionBase previousAction;

        private Phase phase;
        [SerializeField]private bool actionIsDone;
        
     
        public static GameActionExecutor Instance { get; private set; }
        

        #region SetUp
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

        private void HandleWaitingOnUser()
        {
            TryGetNextAction();
        }

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
        
        private void DoCurrentAction()
        {
            currentAction.DoAction();
            Debug.Log($"Do action {currentAction.GetType().Name}");
            actionIsDone = false;
            StartCoroutine(DoActionRoutine(currentAction.Duration));
        }

        private IEnumerator DoActionRoutine(float wait)
        {
            yield return new WaitForSeconds(wait);
            actionIsDone = true;
        }
        
        private void GetNextAction()
        {
            previousAction = currentAction;
            currentAction = null;
            TryGetNextAction();
            if (currentAction == null ) {
                phase = Phase.WaitingOnUser;
            }
        }
        
        
        private void TryGetNextAction()
        {
            if (actions.Any() ) {
                currentAction = actions[0];
                actions.RemoveAt(0);
                phase = Phase.ExecutingAction;
            }
        }

        #endregion


        public void AddToBottom(List<GameActionBase> cardActionDatas)
        {
            foreach (var playerAction in cardActionDatas)
            {
                AddToBottom(playerAction);
            }
        }
        
        public void AddToTop(GameActionBase action) {
            actions.Insert(0, action);
        }
        
        public void AddToBottom(GameActionBase action) {
            actions.Add(action);
        }
    }

    public enum Phase
    {
        WaitingOnUser,
        ExecutingAction
    }
}