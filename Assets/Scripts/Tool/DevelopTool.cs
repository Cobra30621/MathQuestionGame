using System;
using System.Collections.Generic;
using Combat;
using Managers;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.Events;
using NueGames.Combat;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using NueGames.Power;
using Question;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace Tool
{
    /// <summary>
    /// 給遊戲開發者測試用的創造器
    /// </summary>
    public class DevelopTool : MonoBehaviour
    {
        /// <summary>
        /// 測試的事件
        /// </summary>
        [LabelText("遊戲開始時，執行事件")]
        public UnityEvent TestEvent;
        
        [InlineEditor()]
        [LabelText("遊戲設定")]
        public GameplayData GameplayData;
        
        [LabelText("產生的敵人")]
        [InlineEditor()]
        public EnemyEncounter EnemyEncounter;

        [LabelText("問題設定")]
        public QuestionSetting QuestionSetting;
        
        [LabelText("玩家初始獲得的能力")]
        public List<PowerName> allyPowerAtGameStart;
        
        

        void Start()
        {
            SetDevelopModeData();
            
            PlayTest();

            GenerateAllyPower();
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadScene();
            }
        }

        [Button]
        public void ReloadScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        
        [ContextMenu("使用測試方法")]
        public void PlayTest()
        {
            TestEvent.Invoke();
            
        }

        private void SetDevelopModeData()
        {
            GameManager.Instance.SetGameplayData(GameplayData);
            GameManager.Instance.SetEnemyEncounter(EnemyEncounter);
            QuestionManager.Instance.SetQuestionSetting(QuestionSetting); 
            
            GameManager.Instance.StartDevelopMode();
        }
        
        private void GenerateAllyPower()
        {
            foreach (var powerName in allyPowerAtGameStart)
            {
                var ally = CombatManager.Instance.MainAlly;
                
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(1, powerName, 
                    new List<CharacterBase>(){ally}, null));
            }
        }
    }
}