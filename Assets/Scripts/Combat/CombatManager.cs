using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.CharacterAbility;
using NueGames.Characters;
using NueGames.Characters.Enemies;
using NueGames.Data.Encounter;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Power;
using NueGames.Utils.Background;
using Question;
using UnityEngine;

namespace NueGames.Combat
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
        private ManaManager _manaManager;

        public int CurrentMana => _manaManager.CurrentMana;
        public Action<int> OnGainMana
        {
            get => _manaManager.OnGainMana;
            set => _manaManager.OnGainMana = value;
        }

        #region Character
        // 所有敵人清單
        public List<EnemyBase> Enemies { get; private set; }
        // 玩家
        public AllyBase MainAlly;
        
        /// <summary>
        /// 現在正在被選擇中的敵人
        /// </summary>
        public EnemyBase CurrentSelectedEnemy;

        
        #endregion
        
        
        
        #region Cache
        


        private List<Transform> EnemyPosList => enemyPosList;

        private List<Transform> AllyPosList => allyPosList;

        

        public EnemyEncounter CurrentEncounter { get; private set; }
        
        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {
                _currentCombatStateType = value;
                // Debug.Log($"currentCombatState {_currentCombatStateType}");
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
                _manaManager = new ManaManager();
                CurrentCombatStateType = CombatStateType.PrepareCombat;
            }
        }

        private void Start()
        {
            StartCombat();
        }

        
        
        
        #endregion



        #region 會更改 Routines 的方法

        public void StartCombat()
        {
            StartCoroutine(StartCombatRoutine());
        }
        
        public void EndTurn()
        {
            OnTurnEnd?.Invoke(GetTurnInfo(CharacterType.Ally)); // 玩家回合結束
            
            CurrentCombatStateType = CombatStateType.EnemyTurn;
        }
        
        
        private void LoseCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
            
            CurrentCombatStateType = CombatStateType.EndCombat;
            
            StartCoroutine(LoseCombatRoutine());
        }
        private void WinCombat()
        {
            if (CurrentCombatStateType == CombatStateType.EndCombat) return;
          
            CurrentCombatStateType = CombatStateType.EndCombat;

            StartCoroutine(WinCombatRoutine());

        }
        

        #endregion
        
        
        
        
        #region 流程 Routines
        private void ExecuteCombatState(CombatStateType targetStateType)
        {
            switch (targetStateType)
            {
                case CombatStateType.PrepareCombat:
                    break;
                case CombatStateType.RoundStart:
                    StartCoroutine(RoundStartRoutine());
                    break;
                case CombatStateType.AllyTurn:
                    StartCoroutine(AllyTurnRoutine());
                    break;
                case CombatStateType.EnemyTurn:
                    StartCoroutine(EnemyTurnRoutine());
                    break;
                case CombatStateType.EndRound:
                    StartCoroutine(RoundEndRoutine());
                    
                    break;
                case CombatStateType.EndCombat:
                    GameManager.CanSelectCards = false;
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStateType), targetStateType, null);
            }
        }

        private IEnumerator StartCombatRoutine()
        {
            BuildEnemies();
            BuildAllies();
            SetCharacterSkill();

            backgroundContainer.OpenSelectedBackground();

            RoundNumber = 0;
          
            CollectionManager.SetGameDeck();
            QuestionManager.Instance.OnCombatStart();
           
            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.InformationCanvas.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(0.1f);
            
            yield return BattleStartEnemyRoutine();

            CurrentCombatStateType = CombatStateType.RoundStart;
        }

        

        private IEnumerator RoundStartRoutine()
        {
            RoundNumber++;
            
            _manaManager.HandleAtTurnStartMana();
            CollectionManager.DrawCards(GameManager.PlayerData.DrawCount);
            GameManager.CanSelectCards = true;
            
            OnRoundStart.Invoke(GetRoundInfo());
            yield return new WaitForSeconds(0.1f);
            
            CurrentCombatStateType = CombatStateType.AllyTurn;
        }
        
        private IEnumerator AllyTurnRoutine()
        {
            OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Ally));
            
            // TODO 改成等 OnTurnStart 結束觸發
            yield return new WaitForSeconds(0.3f);
                    
            if (MainAlly.GetCharacterStats().IsStunned)
            {
                EndTurn();
            }
        }

        /// <summary>
        /// 回合開始時，敵人行動
        /// </summary>
        /// <returns></returns>
        private IEnumerator BattleStartEnemyRoutine()
        {
            var waitDelay = new WaitForSeconds(0.1f);

            foreach (var currentEnemy in Enemies)
            {
                yield return currentEnemy.StartCoroutine(nameof(EnemyExample.BattleStartActionRoutine));
                yield return waitDelay;
            }
        }
        
        private IEnumerator EnemyTurnRoutine()
        {
            OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Enemy));
            CollectionManager.DiscardHand();

            // TODO 改成等 OnTurnStart 結束觸發
            yield return new WaitForSeconds(0.3f);
            
            var waitDelay = new WaitForSeconds(0.1f);

            foreach (var currentEnemy in Enemies)
            {
                yield return currentEnemy.StartCoroutine(nameof(EnemyExample.ActionRoutine));
                yield return waitDelay;
            }
            
            GameManager.CanSelectCards = false;

            if (CurrentCombatStateType != CombatStateType.EndCombat)
            {
                CurrentCombatStateType = CombatStateType.EndRound;
            }
            else
            {
                OnTurnEnd?.Invoke(GetTurnInfo(CharacterType.Enemy)); // 敵人回合結束
            }
        }

        private IEnumerator RoundEndRoutine()
        {
            OnRoundEnd?.Invoke(GetRoundInfo());
            yield return new WaitForSeconds(0.1f);
            
            CurrentCombatStateType = CombatStateType.RoundStart;
        }

        private IEnumerator LoseCombatRoutine()
        {
            CollectionManager.DiscardHand();
            CollectionManager.DiscardPile.Clear();
            CollectionManager.DrawPile.Clear();
            CollectionManager.HandPile.Clear();
            CollectionManager.HandController.hand.Clear();
            
            yield return new WaitForSeconds(1.5f);
            
            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.CombatCanvas.CombatLosePanel.SetActive(true);
        }

        private IEnumerator WinCombatRoutine()
        {
            GameManager.PlayerData.SetHealth(
                MainAlly.GetCharacterStats().CurrentHealth, MainAlly.GetCharacterStats().MaxHealth);
            
            CollectionManager.ClearPiles();
            
            yield return new WaitForSeconds(1.5f);
           
            MainAlly.ClearAllPower();
            UIManager.CombatCanvas.gameObject.SetActive(false);
            UIManager.RewardCanvas.ShowReward(new List<RewardType>()
            {
                RewardType.Card, RewardType.Gold
            });
            
        }
        
        
        #endregion
        
        
        #region Public Methods
        
        public void OnAllyDeath(AllyBase targetAlly)
        {
            UIManager.InformationCanvas.ResetCanvas();
            LoseCombat();
        }
        public void OnEnemyDeath(EnemyBase targetEnemy)
        {
            Enemies.Remove(targetEnemy);
            if (Enemies.Count<=0)
                WinCombat();
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
                    target = MainAlly;
                    break;
                case ActionTargetType.RandomEnemy:
                    if (Enemies.Count>0)
                        target = Enemies.RandomItem();
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

        public Transform GetMainAllyTransform()
        {
            return MainAlly.transform;
        }


        public void AddMana(int mana)
        {
            _manaManager.AddMana(mana);
        }


        #endregion
        
        #region Private Methods
        private void BuildEnemies()
        {
            CurrentEncounter = GameManager.CurrentEnemyEncounter;
            
            Enemies = new List<EnemyBase>();
            Debug.Log(CurrentEncounter);
            var enemyList = CurrentEncounter.enemyList;
            for (var i = 0; i < enemyList.Count; i++)
            {
                var clone = Instantiate(enemyList[i].EnemyPrefab, EnemyPosList.Count >= i ? EnemyPosList[i] : EnemyPosList[0]);
                clone.SetEnemyData(enemyList[i]);
                clone.BuildCharacter();
                
                
                Enemies.Add(clone);
            }
        }
        private void BuildAllies()
        {
            var clone = Instantiate(GameManager.MainAllyData.prefab, AllyPosList[0]);
            clone.SetCharacterData(GameManager.MainAllyData);
            clone.BuildCharacter();
            MainAlly = clone;
        }
        
        private void SetCharacterSkill()
        {
            CharacterSkillManager.Instance.SetCharacterSkill(MainAlly.GetCharacterSkill());
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