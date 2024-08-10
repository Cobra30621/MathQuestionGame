using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using Feedback;
using NueGames.CharacterAbility;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Encounter;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Utils.Background;
using Question;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    public class CombatManager : SingletonDestroyOnLoad<CombatManager>
    {
        [Header("References")] 
        [SerializeField] private BackgroundContainer backgroundContainer;
        [SerializeField] private List<Transform> enemyPosList;
        [SerializeField] private List<Transform> allyPosList;
        [SerializeField] private ManaManager _manaManager;
        [SerializeField] private EnemyBuilder _enemyBuilder;

        public int CurrentMana => _manaManager.CurrentMana;
        public Action<int> OnGainMana
        {
            get => ManaManager.OnGainMana;
            set => ManaManager.OnGainMana = value;
        }

        #region Character
        // 所有敵人清單
        public List<EnemyBase> Enemies { get; private set; }
        // 玩家
        public AllyBase MainAlly;

        public EnemyBase RandomEnemy => Enemies.RandomItem();
        
        /// <summary>
        /// 現在正在被選擇中的敵人
        /// </summary>
        [FormerlySerializedAs("CurrentSelectedEnemy")] public EnemyBase currentSelectedEnemyBase;

        [SerializeField] private IFeedback allyTurnStartFeedback, enemyTurnStartFeedback;

        
        #endregion
        
        
        
        #region Cache
        


        private List<Transform> EnemyPosList => enemyPosList;

        private List<Transform> AllyPosList => allyPosList;

        
        public EnemyEncounter currentEncounter;
        
        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {
                _currentCombatStateType = value;
                ExecuteCombatState(value);
            }
        }

        /// <summary>
        /// 第幾個遊戲回合
        /// </summary>
        public int RoundNumber;
        
        private CombatStateType _currentCombatStateType;
        protected FxManager FxManager => FxManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        protected CollectionManager CollectionManager => CollectionManager.Instance;

        #endregion


        #region 事件
        
        /// <summary>
        /// 遊戲回合開始
        /// </summary>
        public static Action<RoundInfo> OnRoundStart ;
        /// <summary>
        /// 遊戲回合結束
        /// </summary>
        public static Action<RoundInfo> OnRoundEnd;
        
        /// <summary>
        /// 玩家/敵人回合開始時觸發
        /// </summary>
        public static Action<TurnInfo> OnTurnStart;
        
        /// <summary>
        /// 玩家/敵人回合結束時觸發
        /// </summary>
        public static  Action<TurnInfo> OnTurnEnd;
        

        #endregion
        
        
        protected override void DoAtAwake()
        {
            _manaManager = new ManaManager();
            CurrentCombatStateType = CombatStateType.PrepareCombat;
        }

        private void Start()
        {
            StartCombat();
        }

        
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
            GameManager.CanSelectCards = false;
            
            OnRoundStart?.Invoke(GetRoundInfo());
            yield return new WaitForSeconds(0.1f);
            
            CurrentCombatStateType = CombatStateType.AllyTurn;
        }
        
        private IEnumerator AllyTurnRoutine()
        {
            OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Ally));
            
            allyTurnStartFeedback.Play();
            yield return new WaitForSeconds(allyTurnStartFeedback.FeedbackDuration());
            GameManager.CanSelectCards = true;
            
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
                yield return currentEnemy.BattleStartActionRoutine();
                yield return waitDelay;
            }
        }
        
        private IEnumerator EnemyTurnRoutine()
        {
            OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Enemy));
            CollectionManager.DiscardHand();

            enemyTurnStartFeedback.Play();
            yield return new WaitForSeconds(enemyTurnStartFeedback.FeedbackDuration());
            
            var waitDelay = new WaitForSeconds(0.5f);

            var CoroutineEnemies = new List<EnemyBase>(Enemies) {  };
            
            foreach (var currentEnemy in CoroutineEnemies)
            {
                yield return currentEnemy.ActionRoutine();
                yield return waitDelay;
            }
            
            yield return new WaitForSeconds(0.5f);
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
        public void OnEnemyDeath(EnemyBase targetEnemyBase)
        {
            Enemies.Remove(targetEnemyBase);
            if (Enemies.Count<=0)
                WinCombat();
        }
        
        public void SetSelectedEnemy(EnemyBase selectedEnemy)
        {
            currentSelectedEnemyBase = selectedEnemy;
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
            currentEncounter = GameManager.CurrentEnemyEncounter;
            
            Enemies = new List<EnemyBase>();
            Debug.Log(currentEncounter);
            var enemyList = currentEncounter.enemyList;
            foreach (var enemyData in enemyList)
            {
                var enemy = _enemyBuilder.Build(enemyData, GetEnemyPos());
                
                Enemies.Add(enemy);
            }
        }

        private Transform GetEnemyPos()
        {
            if (EnemyPosList.Count > Enemies.Count)
            {
                return enemyPosList[Enemies.Count ];
            }
            else
            {
                Debug.LogError($"敵人數量超過限制{Enemies.Count + 1}");
                return EnemyPosList[0];
            }
        }
        
        private void BuildAllies()
        {
            var clone = Instantiate(GameManager.MainAllyData.prefab, AllyPosList[0]);
            clone.BuildCharacter(GameManager.MainAllyData);
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