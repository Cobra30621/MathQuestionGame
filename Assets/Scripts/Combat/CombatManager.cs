﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Ally;
using Characters.Enemy;
using Combat.Card;
using Effect;
using Encounter;
using Encounter.Data;
using Feedback;
using GameListener;
using Log;
using Managers;
using Map;
using NueGames.Data.Settings;
using NueTooltip.Core;
using Reward.Data;
using Sirenix.OdinInspector;
using Stage;
using UI;
using UnityEngine;
using Utils.Background;



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


        /// <summary>
        /// 負責記錄一些戰鬥中的資訊
        /// </summary>
        public CombatCounter CombatCounter;
        
        
        #region Mana

        public int MaxMana()
        {
            int rawValue = _gameplayData.MaxMana;
            int mana = CombatCalculator.GainMaxManaValue(rawValue);
            return mana;
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
            int rawDrawCount = _gameplayData.DrawCount;
            
            return CombatCalculator.GetDrawCountValue(rawDrawCount);
        }

        #endregion
        
        
        #region Character

        public Transform GetMainAllyTransform()
        {
            return MainAlly.transform;
        }
        
        public Ally MainAlly => characterHandler.MainAlly;

        public List<Enemy> Enemies => characterHandler.Enemies;
        
        /// <summary>
        /// 取得用於 Effect 的所有敵人
        /// </summary>
        /// <returns></returns>
        public List<CharacterBase> EnemiesForTarget (){
            List<CharacterBase> targets = new List<CharacterBase>();
            var allEnemy = CombatManager.Instance.Enemies;
            targets.AddRange(allEnemy);
            return targets;
        }
        
        public int EnemyCount => Enemies.Count;

        

        #endregion


        #region Cache

        public EnemyEncounter currentEncounter;

        [ShowInInspector]
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

        /// <summary>
        /// 玩家可以選擇卡片
        /// </summary>
        public bool CanSelectCards;

        
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

        public static Action OnBattleStart;
        


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
            CombatCounter = new CombatCounter();
            CombatCounter.Init();
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
            // 玩家無法操作手牌
            CollectionManager.HandController.DisableDragging();
            
            CombatEventTrigger.InvokeOnTurnEnd(GetTurnInfo(CharacterType.Ally));
            OnTurnEnd?.Invoke(GetTurnInfo(CharacterType.Ally)); // 玩家回合結束

            CurrentCombatStateType = CombatStateType.EnemyTurn;
        }


        public void LoseCombat()
        {
            if (IsEndCombat()) return;
            UIManager.InformationCanvas.ResetCanvas();

            CurrentCombatStateType = CombatStateType.EndCombat;

            StartCoroutine(LoseCombatRoutine());
        }

        public void WinCombat()
        {
            if (IsEndCombat()) return;

            CurrentCombatStateType = CombatStateType.EndCombat;

            StartCoroutine(WinCombatRoutine());
        }

        public bool IsEndCombat()
        {
            return CurrentCombatStateType == CombatStateType.EndCombat;
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
                    CanSelectCards = false;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetStateType), targetStateType, null);
            }
        }

        private IEnumerator StartCombatRoutine()
        {
            EventLogger.Instance.LogEvent(LogEventType.Combat, "---------- 戰鬥開始 ----------");

            var encounterName = EncounterManager.Instance.currentEnemyEncounter;
            currentEncounter = enemyEncounterOverview.FindUniqueId(encounterName.Id);
            characterHandler.BuildEnemies(currentEncounter.enemyList);
            characterHandler.BuildAllies(StageSelectedManager.Instance.GetAllyData());

            backgroundContainer.OpenSelectedBackground();

            RoundNumber = 0;

            CollectionManager.SetGameDeck();

            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.InformationCanvas.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.1f);
            OnBattleStart?.Invoke();
            CombatEventTrigger.InvokeOnBattleStart();
            yield return BattleStartEnemyRoutine();

            CurrentCombatStateType = CombatStateType.RoundStart;
        }


        private IEnumerator RoundStartRoutine()
        {
            RoundNumber++;
            _manaManager.HandleAtTurnStartMana();
            CollectionManager.DrawCards(DrawCount());
            CanSelectCards = false;

            EventLogger.Instance.LogEvent(LogEventType.Combat, $"回合 {RoundNumber} 開始");
            OnRoundStart?.Invoke(GetRoundInfo());
            CombatEventTrigger.InvokeOnRoundStart(GetRoundInfo());
            yield return new WaitForSeconds(0.1f);

            CurrentCombatStateType = CombatStateType.AllyTurn;
        }

        private IEnumerator AllyTurnRoutine()
        {
            // 等待遊戲行為序列完成
            yield return new WaitUntil(() => !EffectExecutor.Instance.IsExecuting);
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"玩家階段開始");
            OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Ally));

            yield return new WaitForSeconds(0.1f);
            CombatEventTrigger.InvokeOnTurnStart(GetTurnInfo(CharacterType.Ally));
            
            
            // 玩家可以操作手牌
            CollectionManager.HandController.EnableDragging();
            allyTurnStartFeedback.Play();
            yield return new WaitForSeconds(allyTurnStartFeedback.FeedbackDuration());
            CanSelectCards = true;

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
            yield return new WaitUntil(() => !EffectExecutor.Instance.IsExecuting);
            
            CollectionManager.DiscardHand();
            enemyTurnStartFeedback.Play();
            yield return new WaitForSeconds(enemyTurnStartFeedback.FeedbackDuration());
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"敵人階段開始");
            OnTurnStart?.Invoke(GetTurnInfo(CharacterType.Enemy));
            CombatEventTrigger.InvokeOnTurnStart(GetTurnInfo(CharacterType.Enemy));
            
            // 等 On Turn Start 的行動都撥放完畢
            yield return new WaitUntil(() => !EffectExecutor.Instance.IsExecuting);
            
            var waitDelay = new WaitForSeconds(0.5f);

            var CoroutineEnemies = new List<Enemy>(Enemies) { };

            foreach (var currentEnemy in CoroutineEnemies)
            {
                yield return currentEnemy.SkillRoutine();
                yield return waitDelay;
            }

            yield return new WaitForSeconds(0.5f);
            CanSelectCards = false;
            
            OnTurnEnd?.Invoke(GetTurnInfo(CharacterType.Enemy)); // 敵人回合結束
                CombatEventTrigger.InvokeOnTurnEnd(GetTurnInfo(CharacterType.Enemy));
            
            yield return new WaitForSeconds(0.5f);
            
            if(!IsEndCombat())
                CurrentCombatStateType = CombatStateType.EndRound;
        }

        private IEnumerator RoundEndRoutine()
        {
            OnRoundEnd?.Invoke(GetRoundInfo());
            CombatEventTrigger.InvokeOnRoundEnd(GetRoundInfo());
            yield return new WaitForSeconds(0.1f);

            if(!IsEndCombat())
                CurrentCombatStateType = CombatStateType.RoundStart;
        }

        private IEnumerator LoseCombatRoutine()
        {
            EventLogger.Instance.LogEvent(LogEventType.Combat, "---------- 戰鬥失敗 ----------");
            CombatEventTrigger.InvokeOnBattleLose(RoundNumber);

            HandleEndBattle();

            yield return new WaitForSeconds(1.5f);

            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.CombatCanvas.CombatLosePanel.SetActive(true);
        }

        private IEnumerator WinCombatRoutine()
        {
            EventLogger.Instance.LogEvent(LogEventType.Combat, "---------- 戰鬥勝利 ----------");
            OnBattleWin?.Invoke(RoundNumber);
            CombatEventTrigger.InvokeOnBattleWin(RoundNumber);
            
            GameManager.AllyHealthHandler.SetHealth(
                MainAlly.GetCharacterStats().CurrentHealth);

            HandleEndBattle();

            yield return new WaitForSeconds(1.5f);

            MainAlly.ClearAllPower();
            UIManager.CombatCanvas.gameObject.SetActive(false);
            var currentNodeType = MapManager.Instance.GetCurrentNodeType();
            // 產生戰後獲勝後的獎勵
            UIManager.RewardCanvas.ShowCombatWinReward( currentNodeType);
        }

        private void HandleEndBattle()
        {
            CollectionManager.DiscardHand();
            CollectionManager.ClearPiles();
            
            TooltipManager.Instance.HideTooltip();
            
            CombatCounter.OnBattleEnd();
        }

        

        #endregion

        #region 取得角色資訊

        public Enemy RandomEnemy => characterHandler.RandomEnemy();

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