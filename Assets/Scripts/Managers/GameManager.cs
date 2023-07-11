using System;
using Managers;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using NueGames.NueExtentions;
using NueGames.Power;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : MonoBehaviour
    { 
        public GameManager(){}
        public static GameManager Instance { get; private set; }
        
        [Header("Settings")]
        [InlineEditor()]
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private SceneData sceneData;

        #region Cache
        public SceneData SceneData => sceneData;
        public GameplayData GameplayData => gameplayData;
        [ShowInInspector]
        public PersistentGameplayData PersistentGameplayData { get; private set; }
        
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
        
        public void StartRougeLikeGame()
        {
            InitGameplayData();
            SetInitalHand();
            InitialRelic();
        }
        
        public void InitGameplayData()
        { 
            PersistentGameplayData = new PersistentGameplayData(gameplayData);
            if (UIManager)
                UIManager.InformationCanvas.ResetCanvas();
           
        }

        private void InitialRelic()
        {
            foreach (var relicType in gameplayData.InitialRelic)
            {
                RelicManager.Instance.GainRelic(relicType);
            }
        }
        
        
        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }
        public void SetInitalHand()
        {
            PersistentGameplayData.CurrentCardsList.Clear();
            
            foreach (var cardData in GameplayData.InitalDeck.CardList)
                PersistentGameplayData.CurrentCardsList.Add(cardData);
                
        }
        
        
        public void OnExitApp()
        {
            
        }

        public void SetGameplayData(GameplayData gameplayData)
        {
            this.gameplayData = gameplayData;
        }
        
        
        public void SetEnemyEncounter(EnemyEncounter encounter)
        {
            PersistentGameplayData.CurrentEnemyEncounter  = encounter;
        }
        #endregion
      

    }
}
