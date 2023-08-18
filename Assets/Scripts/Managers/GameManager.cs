using System;
using System.Collections.Generic;
using Data;
using DataPersistence;
using Managers;
using Map;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using NueGames.Relic;
using Question;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : MonoBehaviour, IDataPersistence
    { 
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private ScriptableObjectFileHandler cardDataFileHandler, allyDataFileHandler;
        
        [Header("Settings")]
        [InlineEditor()]
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private SceneData sceneData;

        #region Cache
        public SceneData SceneData => sceneData;
        public GameplayData GameplayData => gameplayData;
        
        [ShowInInspector]
        public PlayerData PlayerData { get; private set; }
        
        public List<CardData> CurrentCardsList;
        
        public AllyData MainAllyData;
        
        public EnemyEncounter CurrentEnemyEncounter;

        public bool CanSelectCards;
        
        #endregion
        
        
        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
               
            }
            else
            {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
            }
        }


        #endregion



        #region Save, Load Data

        public void LoadData(GameData data)
        {
            PlayerData = data.PlayerData;
            CurrentCardsList = cardDataFileHandler.GuidToData<CardData>(data.PlayerData.CardDataGuids);
            MainAllyData = allyDataFileHandler.GuidToData<AllyData>(data.PlayerData.AllyDataGuid);
            // Debug.Log($"Load Card {CurrentCardsList.Count}");
            SetRelicList(data.PlayerData.Relics);
        }

        public void SaveData(GameData data)
        {
            data.PlayerData = PlayerData;
            // Debug.Log($"Save Card {CurrentCardsList.Count}");
            data.PlayerData.CardDataGuids =  cardDataFileHandler.DataToGuid(CurrentCardsList);
            data.PlayerData.AllyDataGuid = allyDataFileHandler.DataToGuid(MainAllyData);
            data.PlayerData.Relics = RelicManager.Instance.GetRelicNames();
        }
        
        #endregion



        #region Start Game

        public void NewGame()
        {
            SaveManager.Instance.NewGame();
            
            Debug.Log("New Game");
            
            MainAllyData = gameplayData.InitialAllyData;
            SetRelicList(gameplayData.InitialRelic);
            CurrentCardsList = new List<CardData>();
            foreach (var cardData in GameplayData.InitalDeck.CardList)
                CurrentCardsList.Add(cardData);
            
            PlayerData = new PlayerData(gameplayData)
            {
                CardDataGuids = cardDataFileHandler.DataToGuid(CurrentCardsList),
                AllyDataGuid = allyDataFileHandler.DataToGuid(MainAllyData),
                Relics = RelicManager.Instance.GetRelicNames()
            };

            QuestionManager.Instance.GenerateQuestions();
            SaveManager.Instance.SaveGame();
        }

        public void ContinueGame()
        {
            QuestionManager.Instance.GenerateQuestions();
        }

        #endregion
        
        
        #region Public Methods
        
        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }

        
        public void SetGameplayData(GameplayData gameplayData)
        {
            this.gameplayData = gameplayData;
        }

        
        public void SetEnemyEncounter(EnemyEncounter encounter)
        {
            CurrentEnemyEncounter  = encounter;
        }

        public void HealAlly(float percent)
        {
            var healthData = PlayerData.AllyHealthData;
            int heal = Mathf.CeilToInt(healthData.MaxHealth * percent);

            PlayerData.SetHealth(
                healthData.CurrentHealth + heal,healthData.MaxHealth);
        }
        
        private void SetRelicList(List<RelicName> relicNames)
        {
            RelicManager.Instance.GainRelic(relicNames);
        }
        #endregion

        
        
        
        
    }
}
