using System;
using System.Collections;
using System.Collections.Generic;
using Card;
using Card.Data;
using Card.Display;
using Characters;
using Effect;
using Effect.Parameters;
using Effect.Sequence;
using Log;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Combat.Card
{
    public class BattleCard : CardBase, I2DTooltipTarget, IPointerDownHandler, IPointerUpHandler
    {
        #region Cache

        public bool IsInactive { get; private set; }
        public bool IsPlayable { get; private set; }
        public bool IsExhausted { get; private set; }

        protected Transform CachedTransform { get; set; }
        protected WaitForEndOfFrame CachedWaitFrame { get; set; }


        public int ManaCost { get; private set; }


        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;

        [Header("3D Settings")] [SerializeField]
        private Canvas canvas;
        
        
        /// <summary>
        /// 卡片效果執行完畢
        /// </summary>
        public static Action<BattleCard> OnCardExecuteCompleted;

        #endregion

        #region Setup

        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
        }

        public override void Init(CardData cardData)
        {
            var cardInfo = CardManager.Instance.cardInfoGetter.CreateCardInfo(cardData);

            IsPlayable = true;
            Init(cardInfo);
            
            SetInitCardManaCost();
            
            _camera = CollectionManager.HandController.cam;
            if (canvas)
                canvas.worldCamera = _camera;
        }

        /// <summary>
        /// 設定卡片的初始魔力消耗
        /// </summary>
        public void SetInitCardManaCost()
        {
            // 計算能力、遺物加成
            ManaCost =  CardManaCalculator.GetCardManaCost(CardLevelInfo.ManaCost);
            
            _cardInfo.ManaCost = ManaCost;
            UpdateCardDisplay();
        }
        

        #endregion


        #region Do Effect Action

        public virtual void Use(List<CharacterBase> targetList)
        {
            if (!IsPlayable) return;
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"使用卡牌: {_cardInfo.CardLevelInfo.TitleLang}");

            HideTooltipInfo();

            SpendMana(ManaCost);

            DoCharacterFeedback(_cardInfo.CardData);
            DoAction(targetList);
            
            CollectionManager.OnCardPlayed(this);
        }

        public void DoAction(List<CharacterBase> specifiedTargets)
        {
            var effects = GetEffects(specifiedTargets);

            var targetList = effects.Count > 0 ? effects[0].TargetList : new List<CharacterBase>();
            EffectExecutor.AddActionWithFX(new FXSequence(
                effects, CardData.FxInfo, targetList, () =>
                {
                    OnCardExecuteCompleted.Invoke(this);
                }));
        }

        private List<EffectBase> GetEffects(List<CharacterBase> specifiedTargets)
        {
            EffectSource effectSource = new EffectSource()
            {
                SourceType = SourceType.Card,
                SourceBattleCard = this,
                SourceCharacter = CombatManager.MainAlly
            };
            
            
            var gameActions = EffectFactory.GetEffects(CardLevelInfo.SkillInfos,
                specifiedTargets, effectSource);

            return gameActions;
        }

        /// <summary>
        /// 執行要撥放的特效
        /// </summary>
        protected void DoCharacterFeedback(CardData cardData)
        {
            if (cardData.UseDefaultAttackFeedback)
            {
                CombatManager.Instance.MainAlly.PlayDefaultAttackFeedback();
            }

            if (cardData.UseCustomFeedback)
            {
                CombatManager.Instance.MainAlly.PlayFeedback(cardData.CustomFeedbackKey);
            }
        }

        #endregion


        #region Card Methods

        public virtual void Discard()
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            CollectionManager.OnCardDiscarded(this);
            
            // 只有物件沒有刪除、隱藏時，才執行丟棄效果
            if(gameObject != null && gameObject.activeInHierarchy)
                StartCoroutine(DiscardRoutine());
        }

        public virtual void Exhaust()
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            IsExhausted = true;
            CollectionManager.OnCardExhausted(this);
            StartCoroutine(ExhaustRoutine());
        }

        protected virtual void SpendMana(int value)
        {
            if (!IsPlayable) return;
            CombatManager.AddMana(-value);
        }

        public virtual void SetInactiveMaterialState(bool isInactive)
        {
            if (!IsPlayable) return;
            if (isInactive == this.IsInactive) return;

            IsInactive = isInactive;
            _cardDisplay.SetPlayable(isInactive);
        }

        #endregion



        #region Routines

        protected virtual IEnumerator DiscardRoutine()
        {
            var timer = 0f;
            transform.SetParent(CollectionManager.HandController.discardTransform);

            var startPos = CachedTransform.localPosition;
            var endPos = Vector3.zero;

            var startScale = CachedTransform.localScale;
            var endScale = Vector3.zero;

            var startRot = CachedTransform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);

            while (true)
            {
                timer += Time.deltaTime * 5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot, endRot, timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);

                if (timer >= 1f) break;

                yield return CachedWaitFrame;
            }

            gameObject.SetActive(false);

            // if (destroy)
            //     Destroy(gameObject);
        }

        protected virtual IEnumerator ExhaustRoutine()
        {
            var timer = 0f;
            transform.SetParent(CollectionManager.HandController.exhaustTransform);

            var startPos = CachedTransform.localPosition;
            var endPos = Vector3.zero;

            var startScale = CachedTransform.localScale;
            var endScale = Vector3.zero;

            var startRot = CachedTransform.localRotation;
            var endRot = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);

            while (true)
            {
                timer += Time.deltaTime * 5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot, endRot, timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);

                if (timer >= 1f) break;

                yield return CachedWaitFrame;
            }

            gameObject.SetActive(false);
            // if (destroy)
            //     Destroy(gameObject);
        }

        #endregion
    }
}