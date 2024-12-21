using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Encounter;
using Enemy;
using Feedback;
using Managers;
using Map;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Encounter;
using NueGames.Data.Settings;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Utils.Background;
using Question;
using Reward;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class CombatManager : SingletonDestroyOnLoad<CombatManager>
    {
        [Header("References")] [SerializeField]
        private BackgroundContainer backgroundContainer;

        [SerializeField] private ManaManager _manaManager;

        [Required] public CharacterHandler characterHandler;
        [Required] public EnemyEncounterOverview enemyEncounterOverview;

        [Required] [SerializeField] private IFeedback allyTurnStartFeedback, enemyTurnStartFeedback;

        [Required]
        [SerializeField] private GameplayData _gameplayData;

        #region Mana

        public int MaxMana()
        {
            return _gameplayData.MaxMana;
        }
        
        public int CurrentMana => _manaManager.CurrentMana;

        public Action<int> OnGainMana
        {
            get => ManaManager.OnGainMana;
            set => ManaManager.OnGainMana = value;
        }

        public void AddMana(int mana)
        {
            _manaManager.AddMana(mana);
        }
        public void SetMana(int mana)
        {
            _manaManager.SetMana(mana);
        }

        #endregion

        #region Draw

        public int DrawCount()
        {
            return _gameplayData.DrawCount;
        }

        #endregion
        
        
        #region Character

        public Transform GetMainAllyTransform()
        {
            return MainAlly.transform;
        }
        
        public Ally MainAlly => characterHandler.MainAlly;

        public List<Enemy.Enemy> Enemies => characterHandler.Enemies;
        
        public int EnemyCount => Enemies.Count;

        

        #endregion


        #region Cache

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

        private CombatStateType _currentCombatStateType;

        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        protected CollectionManager CollectionManager => CollectionManager.Instance;

        #endregion


        #region Turn and round

        /// <summary>
        /// 第幾個遊戲回合
        /// </summary>
        public int RoundNumber;

        /// <summary>
        /// 遊戲回合開始
        /// </summary>
        public static Action<RoundInfo> OnRoundStart;

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
        public static Action<TurnInfo> OnTurnEnd;

        public static Action<int> OnBattleWin;

        public static System.Action OnBattleStart;
        


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


        #region Set Up

        protected override void DoAtAwake()
        {
            _manaManager = new ManaManager();
            CurrentCombatStateType = CombatStateType.PrepareCombat;
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


        public void LoseCombat()
        {
            UIManager.InformationCanvas.ResetCanvas();

            if (CurrentCombatStateType == CombatStateType.EndCombat) return;

            CurrentCombatStateType = CombatStateType.EndCombat;

            StartCoroutine(LoseCombatRoutine());
        }

        public void WinCombat()
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
            
            var encounterName = GameManager.CurrentEnemyEncounter;
            currentEncounter = enemyEncounterOverview.FindUniqueId(encounterName.Id);
            characterHandler.BuildEnemies(currentEncounter.enemyList);
            characterHandler.BuildAllies(GameManager.allyData);

            backgroundContainer.OpenSelectedBackground();

            RoundNumber = 0;

            CollectionManager.SetGameDeck();
            QuestionManager.Instance.OnCombatStart();

            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.InformationCanvas.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.1f);
            OnBattleStart?.Invoke();
            yield return BattleStartEnemyRoutine();

            CurrentCombatStateType = CombatStateType.RoundStart;
        }


        private IEnumerator RoundStartRoutine()
        {
            RoundNumber++;

            _manaManager.HandleAtTurnStartMana();
            CollectionManager.DrawCards(DrawCount());
            GameManager.CanSelectCards = false;

            OnRoundStart?.Invoke(GetRoundInfo());
            yield return new WaitForSeconds(0.1f);

            CurrentCombatStateType = CombatStateType.AllyTurn;
        }

        private IEnumerator AllyTurnRoutine()
        {
            // 等待遊戲行為序列完成
            yield return new WaitUntil(() => !GameActionExecutor.Instance.IsExecuting);
            
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
            // 等待遊戲行為序列完成
            yield return new WaitUntil(() => !GameActionExecutor.Instance.IsExecuting);
            
            OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Enemy));
            CollectionManager.DiscardHand();

            enemyTurnStartFeedback.Play();
            yield return new WaitForSeconds(enemyTurnStartFeedback.FeedbackDuration());

            var waitDelay = new WaitForSeconds(0.5f);

            var CoroutineEnemies = new List<Enemy.Enemy>(Enemies) { };

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
            OnBattleWin?.Invoke(RoundNumber);
            GameManager.AllyHealthHandler.SetHealth(
                MainAlly.GetCharacterStats().CurrentHealth);

            CollectionManager.ClearPiles();

            yield return new WaitForSeconds(1.5f);

            MainAlly.ClearAllPower();
            UIManager.CombatCanvas.gameObject.SetActive(false);
            var currentNodeType = MapManager.Instance.GetCurrentNodeType();
            UIManager.RewardCanvas.ShowReward(new List<RewardData>()
            {
                new()
                {
                    RewardType = RewardType.Card,
                    ItemGainType =  ItemGainType.Character
                }
            }, currentNodeType);
        }

        #endregion

        #region 取得角色資訊

        public Enemy.Enemy RandomEnemy => characterHandler.RandomEnemy();

        public int GetEnemyTotalHealth()
        {
            return Enemies.Sum(enemy => enemy.GetHealth());
        }

        public bool GetEnemyById(string id, out CharacterBase enemy)
        {
            // find in Enemies by enemy.id = id
            foreach (var enemyBase in Enemies)
            {
                if (enemyBase.GetId() == id)
                {
                    enemy = enemyBase;
                    return true;
                }
            }

            enemy = null;
            return false;
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