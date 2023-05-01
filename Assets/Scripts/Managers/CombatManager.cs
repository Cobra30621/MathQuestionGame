using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Characters.Enemies;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.NueExtentions;
using NueGames.Power;
using NueGames.Utils.Background;
using Question;
using UnityEngine;

namespace NueGames.Managers
{
    public class CombatManager : MonoBehaviour
    {
        private CombatManager(Action<TurnInfo> onTurnStart)
        {
            OnTurnStart = onTurnStart;
        }
        public static CombatManager Instance { get; private set; }

        [Header("References")] 
        [SerializeField] private BackgroundContainer backgroundContainer;
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private List<Transform> allyPosList;
 
        
        #region Cache
        public List<EnemyBase> CurrentEnemiesList { get; private set; }
        public List<AllyBase> CurrentAlliesList { get; private set; }
        
        
        public List<Transform> EnemyPosList => enemyPosList;

        public List<Transform> AllyPosList => allyPosList;

        public AllyBase CurrentMainAlly => CurrentAlliesList.Count>0 ? CurrentAlliesList[0] : null;
        public EnemyBase CurrentSelectedEnemy;

        public EnemyEncounter CurrentEncounter { get; private set; }
        
        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {
                _currentCombatStateType = value;
                Debug.Log($"currentCombatState {_currentCombatStateType}");
                ExecuteCombatState(value);
            }
        }

        /// <summary>
        /// 第幾個遊戲回合
        /// </summary>
        public int RoundNumber;
        
        private CombatStateType _currentCombatStateType;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        protected CollectionManager CollectionManager => CollectionManager.Instance;

        #endregion


        #region 事件
        
        /// <summary>
        /// 遊戲回合開始
        /// </summary>
        public Action<RoundInfo> OnRoundStart;
        /// <summary>
        /// 遊戲回合結束
        /// </summary>
        public Action<RoundInfo> OnRoundEnd;
        
        /// <summary>
        /// 玩家/敵人回合開始時觸發
        /// </summary>
        public Action<TurnInfo> OnTurnStart;
        
        /// <summary>
        /// 玩家/敵人回合結束時觸發
        /// </summary>
        public Action<TurnInfo> OnTurnEnd;
        

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
                Instance = this;
                CurrentCombatStateType = CombatStateType.PrepareCombat;
            }
        }

        private void Start()
        {
            StartCombat();
        }

        public void StartCombat()
        {
            BuildEnemies();
            BuildAllies();
            backgroundContainer.OpenSelectedBackground();

            RoundNumber = 0;
          
            CollectionManager.SetGameDeck();
            QuestionManager.Instance.OnCombatStart();
           
            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.InformationCanvas.gameObject.SetActive(true);
            CurrentCombatStateType = CombatStateType.RoundStart;
        }
        
        private void ExecuteCombatState(CombatStateType targetStateType)
        {
            switch (targetStateType)
            {
                case CombatStateType.PrepareCombat:
                    break;
                case CombatStateType.RoundStart:
                    RoundNumber++;
                    OnRoundStart.Invoke(GetRoundInfo());
                    
                    CurrentCombatStateType = CombatStateType.AllyTurn;
                    break;
                case CombatStateType.AllyTurn:
                    OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Ally));
                    
                    if (CurrentMainAlly.CharacterStats.IsStunned)
                    {
                        EndTurn();
                        return;
                    }
                    
                    GameManager.PersistentGameplayData.CurrentMana = GameManager.PersistentGameplayData.MaxMana;
                    CollectionManager.DrawCards(GameManager.PersistentGameplayData.DrawCount);
                    GameManager.PersistentGameplayData.CanSelectCards = true;
                    break;
                case CombatStateType.EnemyTurn:
                    OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Enemy));
                    CollectionManager.DiscardHand();
                    
                    StartCoroutine(nameof(EnemyTurnRoutine));
                    
                    GameManager.PersistentGameplayData.CanSelectCards = false;
                    OnTurnEnd?.Invoke(GetTurnInfo(CharacterType.Enemy)); // 敵人回合結束
                    
                    break;
                case CombatStateType.EndRound:
                    OnRoundEnd?.Invoke(GetRoundInfo());

                    CurrentCombatStateType = CombatStateType.RoundStart;
                    break;
                case CombatStateType.EndCombat:
                   
                    
                    GameManager.PersistentGameplayData.CanSelectCards = false;
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStateType), targetStateType, null);
            }
        }
        #endregion

        #region Public Methods
        public void EndTurn()
        {
            OnTurnEnd?.Invoke(GetTurnInfo(CharacterType.Ally)); // 玩家回合結束
            
            CurrentCombatStateType = CombatStateType.EnemyTurn;
        }
        public void OnAllyDeath(AllyBase targetAlly)
        {
            var targetAllyData = GameManager.PersistentGameplayData.AllyList.Find(x =>
                x.AllyCharacterData.CharacterID == targetAlly.AllyCharacterData.CharacterID);
            if (GameManager.PersistentGameplayData.AllyList.Count>1)
                GameManager.PersistentGameplayData.AllyList.Remove(targetAllyData);
            CurrentAlliesList.Remove(targetAlly);
            UIManager.InformationCanvas.ResetCanvas();
            if (CurrentAlliesList.Count<=0)
                LoseCombat();
        }
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            CurrentEnemiesList.Remove(targetEnemy);
            if (CurrentEnemiesList.Count<=0)
                WinCombat();
        }

        public void IncreaseMana(int target)
        {
            GameManager.PersistentGameplayData.CurrentMana += target;
            UIManager.CombatCanvas.SetPileTexts();
        }


        public CharacterBase EnemyDetermineTargets(CharacterBase enemyCharacter,
            ActionTargetType actionTargetType)
        {
           CharacterBase target = null;
            switch (actionTargetType)
            {
                case ActionTargetType.AllEnemies:
                case ActionTargetType.Enemy:
                    target = enemyCharacter;
                    break;
                case ActionTargetType.Ally:
                    target = CurrentMainAlly;
                    break;
                case ActionTargetType.RandomEnemy:
                    if (CurrentEnemiesList.Count>0)
                        target = CurrentEnemiesList.RandomItem();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return target;
        }
        
        public void SetSelectedEnemy(EnemyBase selectedEnemy)
        {
            CurrentSelectedEnemy = selectedEnemy;
        }

        public Dictionary<PowerType, PowerBase> GetMainAllyPowerDict()
        {
            return CurrentMainAlly.GetPowerDict();
        }

        public void SpendPower(PowerType powerType, int value)
        {
            CurrentMainAlly.CharacterStats.ApplyPower(powerType, - value);
        }
        
        #endregion
        
        #region Private Methods
        private void BuildEnemies()
        {
            CurrentEncounter = GameManager.EncounterData.GetEnemyEncounter(
                GameManager.PersistentGameplayData.CurrentStageId,
                GameManager.PersistentGameplayData.CurrentEncounterId,
                GameManager.PersistentGameplayData.IsFinalEncounter);
            
            CurrentEnemiesList = new List<EnemyBase>();
            var enemyList = CurrentEncounter.EnemyList;
            for (var i = 0; i < enemyList.Count; i++)
            {
                var clone = Instantiate(enemyList[i].EnemyPrefab, EnemyPosList.Count >= i ? EnemyPosList[i] : EnemyPosList[0]);
                clone.BuildCharacter();
                
                CurrentEnemiesList.Add(clone);
            }
        }
        private void BuildAllies()
        {
            CurrentAlliesList = new List<AllyBase>();
            for (var i = 0; i < GameManager.PersistentGameplayData.AllyList.Count; i++)
            {
                var clone = Instantiate(GameManager.PersistentGameplayData.AllyList[i], AllyPosList.Count >= i ? AllyPosList[i] : AllyPosList[0]);
                clone.BuildCharacter();
                
                CurrentAlliesList.Add(clone);
            }
        }

        /// <summary>
        /// 取得玩家/敵人回合資訊
        /// </summary>
        /// <param name="characterType"></param>
        /// <returns></returns>
        private TurnInfo GetTurnInfo(CharacterType characterType)
        {
            return new TurnInfo()
            {
                CharacterType = characterType,
                RoundNumber = RoundNumber
            };
        }

        /// <summary>
        /// 取得遊戲回合資訊
        /// </summary>
        /// <returns></returns>
        private RoundInfo GetRoundInfo()
        {
            return new RoundInfo()
            {
                RoundNumber = RoundNumber
            };
        }
        
        
        private void LoseCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
            
            CurrentCombatStateType = CombatStateType.EndCombat;
            
            CollectionManager.DiscardHand();
            CollectionManager.DiscardPile.Clear();
            CollectionManager.DrawPile.Clear();
            CollectionManager.HandPile.Clear();
            CollectionManager.HandController.hand.Clear();
            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.CombatCanvas.CombatLosePanel.SetActive(true);
        }
        private void WinCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
          
            CurrentCombatStateType = CombatStateType.EndCombat;
           
            foreach (var allyBase in CurrentAlliesList)
            {
                GameManager.PersistentGameplayData.SetAllyHealthData(allyBase.AllyCharacterData.CharacterID,
                    allyBase.CharacterStats.CurrentHealth, allyBase.CharacterStats.MaxHealth);
            }
            
            CollectionManager.ClearPiles();
            
           
            if (GameManager.PersistentGameplayData.IsFinalEncounter)
            {
                UIManager.CombatCanvas.CombatWinPanel.SetActive(true);
            }
            else
            {
                CurrentMainAlly.CharacterStats.ClearAllPower();
                GameManager.PersistentGameplayData.CurrentEncounterId++;
                UIManager.CombatCanvas.gameObject.SetActive(false);
                UIManager.RewardCanvas.gameObject.SetActive(true);
                UIManager.RewardCanvas.PrepareCanvas();
                UIManager.RewardCanvas.BuildReward(RewardType.Gold);
                UIManager.RewardCanvas.BuildReward(RewardType.Card);
            }
           
        }
        #endregion
        
        #region Routines
        private IEnumerator EnemyTurnRoutine()
        {
            var waitDelay = new WaitForSeconds(0.1f);

            foreach (var currentEnemy in CurrentEnemiesList)
            {
                Debug.Log($"CurrentEnemiesList {CurrentEnemiesList.Count}");
                Debug.Log($"{currentEnemy} + EnemyTurnRoutine");
                yield return currentEnemy.StartCoroutine(nameof(EnemyExample.ActionRoutine));
                yield return waitDelay;
            }

            if (CurrentCombatStateType != CombatStateType.EndCombat)
                CurrentCombatStateType = CombatStateType.EndRound;
        }
        
        
        #endregion
    }



    /// <summary>
    /// 玩家/敵人回合資訊
    /// </summary>
    public class TurnInfo
    {
        /// <summary>
        /// 玩家 or 敵人的回合
        /// </summary>
        public CharacterType CharacterType;
        /// <summary>
        /// 第幾個遊戲回合
        /// </summary>
        public int RoundNumber;
    }

    public class RoundInfo
    {
        /// <summary>
        /// 第幾個遊戲回合
        /// </summary>
        public int RoundNumber;
    }
}