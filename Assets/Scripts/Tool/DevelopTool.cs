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
using Sheets;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Tool
{
    /// <summary>
    /// 給遊戲開發者測試用的創造器
    /// </summary>
    public class DevelopTool : MonoBehaviour
    {
        [Title("數據")]
        [LabelText("使用數據")] [EnumToggleButtons]
        [InfoBox("測試數據讀取表單的 CardTest、SkillTest、EnemyTest、EnemyIntentionTest 分頁；第一次使用請先按「下載測試資料」",
            InfoMessageType.Warning, nameof(UseTestData))]
        public SheetSource dataSource = SheetSource.Official;

        private bool UseTestData => dataSource == SheetSource.Test;

        [Button("下載正式資料", ButtonSizes.Medium)]
        [HorizontalGroup("SheetLoad")]
        public void LoadOfficialSheet() => LoadSheet(SheetSource.Official);

        [Button("下載測試資料", ButtonSizes.Medium)]
        [HorizontalGroup("SheetLoad")]
        public void LoadTestSheet() => LoadSheet(SheetSource.Test);

        /// <summary>
        /// 測試的事件
        /// </summary>
        [Title("測試設定")]
        [LabelText("遊戲開始時，執行事件")]
        public UnityEvent TestEvent;
        
        [LabelText("玩家資料")]
        public AllyName allyName;
        
        [LabelText("產生的敵人們")]
        public EncounterName enemyEncounter;

        [LabelText("問題設定")]
        public QuestionSetting QuestionSetting;
        
        [LabelText("玩家初始獲得的能力")]
        public List<PowerName> allyPowerAtGameStart;

        private void Awake()
        {
            Debug.Log("開發者模式");
            // 要在建立角色、敵人與卡牌之前決定使用哪一份數據
            ApplyDataSource();
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


        /// <summary>
        /// 依「使用數據」設定，切換遊戲使用正式或測試數據
        /// </summary>
        private void ApplyDataSource()
        {
            var loader = GetSheetDataLoader();
            SheetDataMode.Use(dataSource, loader != null ? loader.getter : null);
        }

        private void LoadSheet(SheetSource source)
        {
            var loader = GetSheetDataLoader();
            if (loader == null)
            {
                Debug.LogError("開發者測試器底下找不到 SheetDataLoader（表單讀取器）");
                return;
            }

            loader.Load(source);
        }

        private SheetDataLoader GetSheetDataLoader() => GetComponentInChildren<SheetDataLoader>(true);

        private void SetDevelopModeData()
        {
            GameManager.Instance.SetAllyData(allyName);
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