using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using NueGames.NueExtentions;
using NueGames.Power;
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
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private SceneData sceneData;
        [SerializeField] private bool isDevelopCombatMode;
        // TODO 預設 Encounter 之後優化
        public EnemyEncounter DefaultEncounter;

        #region Cache
        public SceneData SceneData => sceneData;
        public GameplayData GameplayData => gameplayData;
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
                if (isDevelopCombatMode)
                {
                    // TODO 一般模式、開發模式串接
                    StartRougeLikeGame();
                }
            }
        }

        #endregion
        
        #region Public Methods

        
        
        public void StartRougeLikeGame()
        {
            InitGameplayData();
            SetInitalHand();
        }
        
        public void InitGameplayData()
        { 
            PersistentGameplayData = new PersistentGameplayData(gameplayData);
            PersistentGameplayData.CurrentEnemyEncounter = DefaultEncounter;
            if (UIManager)
                UIManager.InformationCanvas.ResetCanvas();
           
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
            
            if (PersistentGameplayData.IsRandomHand)
                for (var i = 0; i < GameplayData.RandomCardCount; i++)
                    PersistentGameplayData.CurrentCardsList.Add(GameplayData.AllCardsList.RandomItem());
            else
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
