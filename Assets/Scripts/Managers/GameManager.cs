using System;
using System.Collections.Generic;
using Card;
using Card.Data;
using Data;
using Data.Encounter;
using DataPersistence;
using Map;
using Coin;
using Money;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using NueGames.Encounter;
using NueGames.Relic;
using Question;
using Relic;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;

namespace NueGames.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : Singleton<GameManager>, IDataPersistence
    {
        [SerializeField] private ScriptableObjectFileHandler allyDataFileHandler, gameplayDataFileHandler;

        [SerializeField] private RelicManager _relicManager;
        public RelicManager RelicManager => _relicManager;
        
        [Header("Settings")]
        [InlineEditor()]
        [Required]
        [SerializeField] private GameplayData gameplayData;

        [Required]
        [SerializeField] private StageSelectedHandler _stageSelectedHandler;

        
        
        #region Cache
        public GameplayData GameplayData => gameplayData;
  
        
        public EncounterName CurrentEnemyEncounter;

        public bool CanSelectCards;

        public StageSelectedHandler StageSelectedHandler => _stageSelectedHandler;

        public AllyData allyData => _stageSelectedHandler.GetAllyData();

        public AllyHealthData AllyHealthData;
        
        #endregion

        #region 檢查第一次進入遊戲

        protected override void DoAtAwake()
        {
            base.DoAtAwake();
            CheckFirstEnterGame();
        }


        private void CheckFirstEnterGame()
        {
            if (SaveManager.Instance.IsFirstEnterGame())
            {
                CoinManager.Instance.SetMoney(gameplayData.InitMoney);
                CoinManager.Instance.SetStone(gameplayData.InitStone);
                CardManager.Instance.CardLevelHandler.InitDictionary();
                RelicManager.relicLevelHandler.InitRelicLevels();
                
                SaveManager.Instance.SavePermanentGame();
                SaveManager.Instance.SetHaveEnterGame();
            }
            else
            {
                SaveManager.Instance.LoadPermanentGame();
            }
        }
        

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


        #region Start Game

        public void NewGame()
        {
            SaveManager.Instance.ClearGameData();
            Debug.Log("New Game");

            SetInitData();
            SaveManager.Instance.SaveSingleGame();
        }

        public void StartDevelopMode()
        {
            SetInitData();
        }

        private void SetInitData()
        {
            _relicManager.GainRelic(allyData.initialRelic);
            CardManager.Instance.SetInitCard(allyData.InitialDeck.CardList);
            MapManager.Instance.Initialized(_stageSelectedHandler.GetStageData());
            QuestionManager.Instance.GenerateQuestions();
            
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
