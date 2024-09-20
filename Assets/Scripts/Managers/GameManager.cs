using System;
using CampFire;
using Card;
using Card.Data;
using Data;
using Data.Encounter;
using DataPersistence;
using Map;
using Money;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.Data.Settings;
using NueGames.Encounter;
using NueGames.Managers;
using NueTooltip.Core;
using Question;
using Relic;
using Save;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>, IDataPersistence
    {
        [Title("設定")] 
        
        [LabelText("開發者模式")]
        [SerializeField] private bool isDevelopMode;
        
        
        [InlineEditor()]
        [Required]
        [SerializeField] private GameplayData gameplayData;
        
        
        [Title("工具")] 
        [SerializeField] private ScriptableObjectFileHandler allyDataFileHandler, gameplayDataFileHandler;
        
        [Required]
        [SerializeField] private StageSelectedHandler _stageSelectedHandler;

        #region Manager
        
        [Title("管理器")] 
        [Required]
        public RelicManager RelicManager;

        [Required]
        public AudioManager AudioManager;

        [Required]
        public FxManager FxManager;

        [Required]
        public TooltipManager TooltipManager;
        
        [Required]
        public GameActionExecutor GameActionManager;

        [Required]
        public MapManager MapManager;

        [Required]
        public EncounterManager EncounterManager;

        [Required]
        public CoinManager CoinManager;

        [Required]
        public CardManager CardManager;

        [Required]
        public SaveManager SaveManager;

        [Required]
        public UIManager UIManager;
        
        [Required]
        public QuestionManager QuestionManager;

        [Required] public CampFireManager CampFireManager;
        
        #endregion
        
        
        #region Cache
        public GameplayData GameplayData => gameplayData;
  
        
        public EncounterName CurrentEnemyEncounter;

        public bool CanSelectCards;

        public StageSelectedHandler StageSelectedHandler => _stageSelectedHandler;

        public AllyData allyData => _stageSelectedHandler.GetAllyData();

        public AllyHealthData AllyHealthData;

        public bool IsDeveloperMode => isDevelopMode;
        
        #endregion

        
        
        
        #region Save, Load Data

        public void LoadData(GameData data)
        {
            AllyHealthData = data.AllyHealthData;
            
            _stageSelectedHandler.SetAllyData(
                allyDataFileHandler.GuidToData<AllyData>(data.AllyDataGuid));

        }

        public void SaveData(GameData data)
        {
            data.AllyHealthData = AllyHealthData;
            
            data.AllyDataGuid = allyDataFileHandler.DataToGuid(
                _stageSelectedHandler.GetAllyData());
        }
        
        #endregion


        #region Start Single Game
        public void NewGame()
        {
            SaveManager.Instance.ClearGameData();
            Debug.Log("New Game");

            CreateSingleGameData();
            SaveManager.Instance.SaveSingleGame();
        }

        public void StartDevelopMode()
        {
            CreateSingleGameData();
        }

        private void CreateSingleGameData()
        {
            RelicManager.GainRelic(allyData.initialRelic);
            CardManager.Instance.SetInitCard(allyData.InitialDeck.CardList);
            MapManager.Instance.Initialized(_stageSelectedHandler.GetStageData());
            QuestionManager.Instance.GenerateQuestions();
            AllyHealthData = new AllyHealthData(allyData.MaxHealth);
            
            UIManager.Instance.RewardCanvas.SetCardReward(allyData.CardRewardData);
        }

        public void ContinueGame()
        {
            SaveManager.Instance.LoadSingleGame();
            
            UIManager.Instance.RewardCanvas.SetCardReward(allyData.CardRewardData);
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
            var healthData = AllyHealthData;
            int heal = Mathf.CeilToInt(healthData.MaxHealth * percent);

            int afterHealHp = Math.Min(healthData.CurrentHealth + heal, healthData.MaxHealth);
            
            AllyHealthData.SetHealth(afterHealHp
                ,healthData.MaxHealth);
            UIManager.Instance.InformationCanvas.ResetCanvas();
        }
        
      
        

        public float GetMoneyDropRate()
        {
            return _stageSelectedHandler.GetMoneyDropRate();
        }

        
        #endregion
        
    }
}
