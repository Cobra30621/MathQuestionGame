using System;
using System.Collections.Generic;
using Card.Data;
using Data;
using Data.Encounter;
using DataPersistence;
using Managers;
using Map;
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
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;

namespace NueGames.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : Singleton<GameManager>, IDataPersistence
    {
        [SerializeField] private ScriptableObjectFileHandler cardDataFileHandler, allyDataFileHandler, gameplayDataFileHandler;

        [SerializeField] private RelicManager _relicManager;
        public RelicManager RelicManager => _relicManager;
        
        [Header("Settings")]
        [InlineEditor()]
        [SerializeField] private GameplayData gameplayData;

        [Required]
        [SerializeField] private StageSelectedHandler _stageSelectedHandler;

        
        
        #region Cache
        public GameplayData GameplayData => gameplayData;
        
        [ShowInInspector]
        public PlayerData PlayerData { get; private set; }
        
        public List<CardData> CurrentCardsList;
        
        public EncounterName CurrentEnemyEncounter;

        public bool CanSelectCards;

        public StageSelectedHandler StageSelectedHandler => _stageSelectedHandler;

        public AllyData allyData => _stageSelectedHandler.GetAllyData();
    
        
        #endregion
        
        
        #region Save, Load Data

        public void LoadData(GameData data)
        {
            PlayerData = data.PlayerData;
            gameplayData = gameplayDataFileHandler.GuidToData<GameplayData>(data.GamePlayDataId);
            CurrentCardsList = cardDataFileHandler.GuidToData<CardData>(data.PlayerData.CardDataGuids);
            _stageSelectedHandler.SetAllyData(
                allyDataFileHandler.GuidToData<AllyData>(data.PlayerData.AllyDataGuid));
            SetRelicList(data.PlayerData.Relics);
        }

        public void SaveData(GameData data)
        {
            data.PlayerData = PlayerData;
            data.GamePlayDataId = gameplayDataFileHandler.DataToGuid(gameplayData);
            data.PlayerData.CardDataGuids =  cardDataFileHandler.DataToGuid(CurrentCardsList);
            data.PlayerData.AllyDataGuid = allyDataFileHandler.DataToGuid(
                _stageSelectedHandler.GetAllyData());
            data.PlayerData.Relics = _relicManager.GetRelicNames();
        }
        
        #endregion



        #region Start Game

        public void NewGame()
        {
            SaveManager.Instance.ClearGameData();
            Debug.Log("New Game");

            SetInitData();
            SaveManager.Instance.SaveGame();
        }

        public void StartDevelopMode()
        {
            SetInitData();
        }

        private void SetInitData()
        {
            SetRelicList(allyData.initialRelic);
            CurrentCardsList = new List<CardData>();
            foreach (var cardData in allyData.InitialDeck.CardList)
                CurrentCardsList.Add(cardData);
            
            PlayerData = new PlayerData(gameplayData, allyData)
            {
                CardDataGuids = cardDataFileHandler.DataToGuid(CurrentCardsList),
                AllyDataGuid = allyDataFileHandler.DataToGuid(allyData),
                Relics = _relicManager.GetRelicNames(),
            };
            
            MoneyManager.Instance.SetMoney(gameplayData.InitMoney);
            
            UIManager.Instance.RewardCanvas.SetCardReward(allyData.CardRewardData);

            MapManager.Instance.Initialized(_stageSelectedHandler.GetStageData());
            QuestionManager.Instance.GenerateQuestions();
        }

        public void ContinueGame()
        {
            SaveManager.Instance.LoadGame();
            
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

        
        public void SetGameplayData(GameplayData gameplayData)
        {
            this.gameplayData = gameplayData;
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
            var healthData = PlayerData.AllyHealthData;
            int heal = Mathf.CeilToInt(healthData.MaxHealth * percent);

            int afterHealHp = Math.Min(healthData.CurrentHealth + heal, healthData.MaxHealth);
            
            PlayerData.SetHealth(afterHealHp
                ,healthData.MaxHealth);
            UIManager.Instance.InformationCanvas.ResetCanvas();
        }
        
        private void SetRelicList(List<RelicName> relicNames)
        {
            _relicManager.GainRelic(relicNames);
        }

        public void ThrowCard(CardData cardData)
        {
            CurrentCardsList.Remove(cardData);
        }

        
        #endregion


        
    }
}
