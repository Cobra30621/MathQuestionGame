using System.Collections.Generic;
using Managers;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.Relic;
using UnityEngine;
using UnityEngine.Events;
using NueGames.Combat;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using Sirenix.OdinInspector;

namespace Tool
{
    /// <summary>
    /// 給遊戲開發者測試用的創造器
    /// </summary>
    public class DevelopTool : MonoBehaviour
    {
        [SerializeField] private bool isDevelopMode;
        /// <summary>
        /// 測試的事件
        /// </summary>
        public UnityEvent TestEvent;
        
        [InlineEditor()]
        public GameplayData GameplayData;
        [InlineEditor()]
        public EnemyEncounter EnemyEncounter;
        
        public List<PowerType> allyPowerAtGameStart;

        void Awake()
        {
            if (isDevelopMode)
            {
                SetDevelopModeData();
            }
        }

        void Start()
        {
            PlayTest();

            if (isDevelopMode)
            {
                GenerateAllyPower();
            }
        }
        
        [ContextMenu("使用測試方法")]
        public void PlayTest()
        {
            TestEvent.Invoke();
            
        }

        private void SetDevelopModeData()
        {
            GameManager.Instance.SetGameplayData(GameplayData);
            GameManager.Instance.StartRougeLikeGame();
            
            GameManager.Instance.SetEnemyEncounter(EnemyEncounter);
            
            
        }
        
        private void GenerateAllyPower()
        {
            foreach (var powerType in allyPowerAtGameStart)
            {
                var ally = CombatManager.Instance.CurrentMainAlly;
                var parameter = new ApplyPowerParameters(ally, powerType, 1);
                
                GameActionExecutor.Instance.DoApplyPowerAction(parameter);
                
                
            }
        }
    }
}