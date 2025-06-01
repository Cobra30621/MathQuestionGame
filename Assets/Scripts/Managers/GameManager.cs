using System;
using Aduio;
using CampFire;
using Card;
using Card.Data;
using Characters.Ally;
using Combat.Card;
using Economy;
using Effect;
using Encounter;
using Feedback;
using Log;
using Map;
using MapEvent;
using NueGames.Data.Settings;
using NueTooltip.Core;
using Question;
using Relic;
using Reward;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using Stage;
using UI;
using UnityEngine;
using Utils;
using VersionControl;

namespace Managers
{
    /// <summary>
    /// 主要的遊戲管理器，負責提供各個子系統的服務
    /// </summary>
    public class GameManager : Singleton<GameManager>, IDataPersistence
    {
        #region 子系統
        [Title("存檔相關")] 
        [Required] public SaveManager SaveManager;
        [Required] [SerializeField] private SystemGameVersion systemGameVersion;
        [Required] public StageSelectedManager stageSelectedManager;
        
        [Title("物品相關")] 
        [Required] public RelicManager RelicManager;
        [Required] public CoinManager CoinManager;
        [Required] public CardManager CardManager;
        [Required] public RewardManager RewardManager;
        
        [Title("地圖相關")] 
        [Required] public MapManager MapManager;
        [Required] public EncounterManager EncounterManager;
        [Required] public EventManager EventManager;
        
        [Title("特效與反饋相關")] 
        [Required] public AudioManager AudioManager;
        [Required] public FxManager FxManager;
        [Required] public TooltipManager TooltipManager;
        
        
        [Title("戰鬥相關")] 
        [Required] public EffectExecutor effectManager;
        [Required] public AllyHealthHandler AllyHealthHandler;

        
        [Title("其他相關")] 
        [Required] [SerializeField] private SceneChanger _sceneChanger;
        [Required] public EventLogger EventLogger;
        
        [Required] public UIManager UIManager;
        [Required] public QuestionManager QuestionManager;
        
       
        

        #endregion
        #region Cache
        [Title("設定")] 
        
        [LabelText("開發者模式")]
        [SerializeField] private bool isDevelopMode;
        
        /// <summary>
        /// 開發者模式
        /// </summary>
        public bool IsDeveloperMode => isDevelopMode;

        
        /// <summary>
        /// 遊戲基礎設定
        /// </summary>
        [InlineEditor()] [Required]
        [LabelText("遊戲基礎設定")]
        public GameplayData GameplayData;

        [Required]
        public AllyDataOverview allyDataOverview;
        

        #endregion

        
        
        
        #region Save, Load Data

        /// <summary>
        /// 目前的遊戲版本
        /// </summary>
        /// <returns></returns>
        public GameVersion SystemVersion()
        {
            return systemGameVersion.systemVersion;
        }
        
        public void LoadData(GameData data)
        {
            AllyHealthHandler.SetAllyHealthData(data.AllyHealthData);
            
            stageSelectedManager.SetAllyData(data.AllyName);

        }

        public void SaveData(GameData data)
        {
            data.AllyHealthData = AllyHealthHandler.GetAllyHealthData();

            data.AllyName = stageSelectedManager.GetCurrentAllyName();
        }
        
        #endregion


        #region Start Single Game
        public void NewGame()
        {
            SaveManager.ClearSingleGameData();
            
            CreateSingleGameData();
            
            SaveManager.SetOngoingGame();
            SaveManager.SaveSingleGame();

            StartCoroutine(_sceneChanger.OpenMapScene());
        }

        public void StartDevelopMode()
        {
            CreateSingleGameData();
        }

        private void CreateSingleGameData()
        {
            var allyData = stageSelectedManager.GetAllyData();
            EventLogger.LogEvent(LogEventType.Main, "創建 - 新的單局遊戲",
                $"角色 : {allyData.CharacterName}\n" +
                $"關卡 : {stageSelectedManager.GetStageData().Id}");
            
            RelicManager.GainRelic(allyData.initialRelic);
            CardManager.playerDeckHandler.SetInitCard(allyData.InitialDeck.CardList);
            MapManager.Initialized(stageSelectedManager.GetStageData());
            AllyHealthHandler.Init(allyData.MaxHealth);
        }

        public void ContinueGame()
        {
            EventLogger.LogEvent(LogEventType.Main, "繼續 - 單局遊戲");
            
            SaveManager.LoadSingleGame();
            StartCoroutine(_sceneChanger.OpenMapScene());
        }

        /// <summary>
        /// 離開單局遊戲
        /// </summary>
        public void ExitSingleGame()
        {
            EventLogger.LogEvent(LogEventType.Main, "離開 - 單局遊戲");
            
            SaveManager.SaveSingleGame();
            RelicManager.RemoveAllRelic();
            _sceneChanger.OpenMainMenuScene();
        }

        #endregion
        
        
        #region Public Methods
        
        public BattleCard BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.BattleCardPrefab, parent);
            clone.Init(targetData);
            return clone;
        }
        
        
        public void SetAllyData(AllyName allyName)
        {
            stageSelectedManager.SetAllyData(allyName);
        }

        
        
        public void HealAlly(float percent)
        {
            AllyHealthHandler.HealByPercent(percent);
            UIManager.InformationCanvas.ResetCanvas();
        }
        
        
        public float GetDifficultyMoneyDropRate()
        {
            return stageSelectedManager.GetMoneyDropRate();
        }

        
        #endregion
        
    }
}
