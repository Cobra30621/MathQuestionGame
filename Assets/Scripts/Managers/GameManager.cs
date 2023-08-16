using System;
using System.Collections.Generic;
using Data;
using DataPersistence;
using Managers;
using Map;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using Question;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : MonoBehaviour, IDataPersistence
    { 
        public GameManager(){}
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private CardDataFileHandler cardDataFileHandler;
        
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
        
        public AllyBase MainAlly;
        public EnemyEncounter CurrentEnemyEncounter;

        
        #endregion
        
        // TODO　整合並去除
        // ReSharper disable once MemberCanBePrivate.Global
        protected UIManager UIManager => UIManager.Instance;
        
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
        
        #region Public Methods
        
        public void NewGame()
        {
            SaveManager.Instance.NewGame();
            
            Debug.Log("Start RougeLikeGame");
            PlayerData = new PlayerData(gameplayData);
            MainAlly = gameplayData.InitialAlly;
            
            foreach (var relicType in gameplayData.InitialRelic)
            {
                RelicManager.Instance.GainRelic(relicType);
            }
            
            CurrentCardsList = new List<CardData>();
            
            foreach (var cardData in GameplayData.InitalDeck.CardList)
                CurrentCardsList.Add(cardData);
            
            QuestionManager.Instance.GenerateQuestions();
        }

        public void ContinueGame()
        {
            QuestionManager.Instance.GenerateQuestions();
        }
        
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
        #endregion

        
        public void LoadData(GameData data)
        {
            PlayerData = data.PlayerData;
            CurrentCardsList = cardDataFileHandler.GuidToData(data.CardDataGuids);
        }

        public void SaveData(GameData data)
        {
            data.PlayerData = PlayerData;
            data.CardDataGuids =  cardDataFileHandler.DataToGuid(CurrentCardsList);
            
        }
    }
}
