using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Data.Settings;
using NueGames.EnemyBehaviour;
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
        [SerializeField] private EncounterData encounterData;
        [SerializeField] private SceneData sceneData;
        [SerializeField] private bool isDevelopCombatMode;


        #region Cache
        public SceneData SceneData => sceneData;
        public EncounterData EncounterData => encounterData;
        public GameplayData GameplayData => gameplayData;
        public PersistentGameplayData PersistentGameplayData { get; private set; }
        protected UIManager UIManager => UIManager.Instance;
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
                EnemyActionProcessor.Initialize();
                PowerFactory.Initialize();
                if (isDevelopCombatMode)
                {
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
        public void NextEncounter()
        {
            PersistentGameplayData.CurrentEncounterId++;
            if (PersistentGameplayData.CurrentEncounterId>=EncounterData.EnemyEncounterList[PersistentGameplayData.CurrentStageId].EnemyEncounterList.Count)
            {
                PersistentGameplayData.CurrentEncounterId = Random.Range(0,
                    EncounterData.EnemyEncounterList[PersistentGameplayData.CurrentStageId].EnemyEncounterList.Count);
            }
        }
        public void OnExitApp()
        {
            
        }

        public void SetGameplayData(GameplayData gameplayData)
        {
            this.gameplayData = gameplayData;
        }
        #endregion
      

    }
}
