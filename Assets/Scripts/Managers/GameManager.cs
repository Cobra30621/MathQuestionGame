using System;
using Aduio;
using CampFire;
using Card;
using Card.Data;
using Characters;
using Characters.Ally;
using Combat.Card;
using Economy;
using Effect;
using Encounter;
using Encounter.Data;
using Feedback;
using Log;
using Map;
using MapEvent;
using NueGames.Card;
using NueGames.Data.Settings;
using NueTooltip.Core;
using Question;
using Relic;
using Reward;
using Save;
using Save.Data;
using Save.FileHandler;
using Sirenix.OdinInspector;
using Stage;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
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
        [Required] public SystemGameVersion SystemGameVersion;
        [Required] [SerializeField] private ScriptableObjectFileHandler allyDataFileHandler;
        [Required] [SerializeField] private StageSelectedHandler _stageSelectedHandler;
        
        [Title("物品相關")] 
        [Required] public RelicManager RelicManager;
        [Required] public CoinManager CoinManager;
        [Required] public CardManager CardManager;
        [Required] public RewardManager RewardManager;
        
        [Title("地圖相關")] 
        [Required] public MapManager MapManager;
        [Required] public EncounterManager EncounterManager;
        [Required] public CampFireManager CampFireManager;
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
        
        
        [InlineEditor()]
        [Required]
        [SerializeField] private GameplayData gameplayData;
        
        public GameplayData GameplayData => gameplayData;
  
        
        public EncounterName CurrentEnemyEncounter;

        public bool CanSelectCards;

        public StageSelectedHandler StageSelectedHandler => _stageSelectedHandler;

        public AllyData allyData => _stageSelectedHandler.GetAllyData();

        
        public bool IsDeveloperMode => isDevelopMode;
        

        #endregion

        
        
        
        #region Save, Load Data

        /// <summary>
        /// 目前的遊戲版本
        /// </summary>
        /// <returns></returns>
        public GameVersion SystemVersion()
        {
            return SystemGameVersion.systemVersion;
        }
        
        public void LoadData(GameData data)
        {
            AllyHealthHandler.SetAllyHealthData(data.AllyHealthData);
            
            _stageSelectedHandler.SetAllyData(
                allyDataFileHandler.GuidToData<AllyData>(data.AllyDataGuid));

        }

        public void SaveData(GameData data)
        {
            data.AllyHealthData = AllyHealthHandler.GetAllyHealthData();
            
            data.AllyDataGuid = allyDataFileHandler.DataToGuid(
                _stageSelectedHandler.GetAllyData());
        }
        
        #endregion


        #region Start Single Game
        public void NewGame()
        {
            SaveManager.Instance.ClearSingleGameData();
            
            CreateSingleGameData();
            
            SaveManager.Instance.SetOngoingGame();
            SaveManager.Instance.SaveSingleGame();
        }

        public void StartDevelopMode()
        {
            CreateSingleGameData();
        }

        private void CreateSingleGameData()
        {
            EventLogger.Instance.LogEvent(LogEventType.Main, "創建 - 新的單局遊戲",
                $"角色 : {allyData.CharacterName}\n" +
                $"關卡 : {_stageSelectedHandler.GetStageData().Id}");
            
            RelicManager.GainRelic(allyData.initialRelic);
            CardManager.Instance.SetInitCard(allyData.InitialDeck.CardList);
            MapManager.Instance.Initialized(_stageSelectedHandler.GetStageData());
            AllyHealthHandler.Init(allyData.MaxHealth);
        }

        public void ContinueGame()
        {
            EventLogger.Instance.LogEvent(LogEventType.Main, "繼續 - 單局遊戲");
            
            SaveManager.Instance.LoadSingleGame();
        }

        /// <summary>
        /// 離開單局遊戲
        /// </summary>
        public void ExitSingleGame()
        {
            EventLogger.Instance.LogEvent(LogEventType.Main, "離開 - 單局遊戲");
            
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
        
        
        public void SetAllyData(AllyData allyData)
        {
            _stageSelectedHandler.SetAllyData(allyData);
        }

        
        public void SetEnemyEncounter(EncounterName encounter)
        {
            CurrentEnemyEncounter  = encounter;
            // Debug.Log($"CurrentEnemyEncounter {CurrentEnemyEncounter.name}");
        }

        public void HealAlly(float percent)
        {
            AllyHealthHandler.HealByPercent(percent);
            UIManager.Instance.InformationCanvas.ResetCanvas();
        }
        
      
        

        public float GetMoneyDropRate()
        {
            return _stageSelectedHandler.GetMoneyDropRate();
        }

        
        #endregion
        
    }
}
