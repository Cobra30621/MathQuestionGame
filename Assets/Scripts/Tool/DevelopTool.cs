using System;
using System.Collections.Generic;
using Characters;
using Characters.Ally;
using Combat;
using Effect;
using Effect.Power;
using Encounter;
using Encounter.Data;
using Managers;
using NueGames.Enums;
using UnityEngine;
using UnityEngine.Events;
using NueGames.Data.Settings;
using Power;
using Question;
using Question.Data;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
        [LabelText("玩家資料")]
        public AllyData allyData;
        
        [LabelText("產生的敵人們")]
        public EncounterName enemyEncounter;

        [LabelText("問題設定")]
        public QuestionSetting QuestionSetting;
        
        [LabelText("玩家初始獲得的能力")]
        public List<PowerName> allyPowerAtGameStart;

        private void Start()
        {
            Debug.Log("開發者模式");
            if(GameManager.Instance.IsDeveloperMode)
                StartDevelopMode();
        }


        [Button]
        public void StartDevelopMode()
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
            GameManager.Instance.SetAllyData(allyData);
            EncounterManager.Instance.SetEnemyEncounter(enemyEncounter);
            QuestionManager.Instance.SetQuestionSetting(QuestionSetting); 
            
            GameManager.Instance.StartDevelopMode();
        }
        
        private void GenerateAllyPower()
        {
            foreach (var powerName in allyPowerAtGameStart)
            {
                var ally = CombatManager.Instance.MainAlly;
                
                EffectExecutor.AddEffect(
                    new ApplyPowerEffect(1, powerName, 
                    new List<CharacterBase>(){ally}, null));
            }
        }
    }
}