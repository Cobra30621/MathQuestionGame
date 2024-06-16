using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using Card;
using Card.Data;
using Card.Display;
using GameAction;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using NueGames.Combat;
using NueGames.Parameters;

namespace NueGames.Card
{
    public class BattleCard : CardBase,I2DTooltipTarget, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Base References")]
        
        [SerializeField] protected Dictionary<RarityType, SingleCardDisplay> cardUIDictionary;
        protected SingleCardDisplay CurrentSingleCard;


        #region Cache
        public bool IsInactive { get; private set; }
        public bool IsPlayable { get; private set; }
        public bool IsExhausted { get; private set; }
        
        protected Transform CachedTransform { get; set; }
        protected WaitForEndOfFrame CachedWaitFrame { get; set; }
        
        
        public int ManaCost { get; private set; }
        
        

        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        
        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;
        
        #endregion
        
        #region Setup
        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
        }

        public override void Init(CardData cardData)
        {
            var cardInfo = CardManager.Instance.CreateCardInfo(cardData);

            IsPlayable = true;
            Init(cardInfo);
            
            ManaCost = CardLevelInfo.Mana;
            
            _camera = CollectionManager.HandController.cam;
            if (canvas)
                canvas.worldCamera = _camera;
        }

   
        #endregion


        #region Do Effect Action

        public virtual void Use(List<CharacterBase> targetList)
        {
            if (!IsPlayable) return;

            HideTooltipInfo(TooltipManager.Instance);
            
            SpendMana(ManaCost);
            
            DoFXAction(_cardInfo.CardData, targetList);
            DoAction(targetList);
            
            CollectionManager.OnCardPlayed(this);
        }

        public void DoAction(List<CharacterBase> targetList)
        {
            ActionSource actionSource = new ActionSource()
            {
                SourceType = SourceType.Card,
                SourceBattleCard = this,
                SourceCharacter = CombatManager.MainAlly
            };
            
            foreach (var effectInfo in CardLevelInfo.EffectInfos)
            {
                var gameAction = GameActionFactory.GetGameAction(effectInfo, targetList, actionSource);
                GameActionExecutor.AddToBottom(gameAction);
            }
        }
        
        /// <summary>
        /// 執行要撥放的特效
        /// </summary>
        protected void DoFXAction(CardData cardData, List<CharacterBase> targetList)
        {
            GameActionExecutor.AddToBottom(new FXAction(
                new FxInfo(cardData.FxGo, cardData.FxSpawnPosition)
                , targetList));

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
        public bool ActionTargetIsSingleEnemy()
        {
            return CardLevelInfo.ActionTargetType == ActionTargetType.Enemy;
        }
        
        
        public virtual void Discard()
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            CollectionManager.OnCardDiscarded(this);
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
            CurrentSingleCard.SetPlayable(isInactive);
        }
        
        

        
        #endregion
        

        #region Card Cost(卡牌花費改變)
        /// <summary>
        /// 降低魔力花費
        /// </summary>
        /// <param name="reduceAmount"></param>
        public void ReduceManaCost(int reduceAmount)
        {
            ManaCost -= reduceAmount;
            if (ManaCost < 0)
                ManaCost = 0;

            _cardInfo.ManaCost = ManaCost;
            UpdateCardDisplay();
        }

        /// <summary>
        /// 設置魔力花費
        /// </summary>
        /// <param name="cost"></param>
        public void SetManaCost(int cost)
        {
            ManaCost = cost;
            _cardInfo.ManaCost = ManaCost;
            UpdateCardDisplay();
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
                timer += Time.deltaTime*5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);
                
                if (timer>=1f)  break;
                
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
                timer += Time.deltaTime*5;

                CachedTransform.localPosition = Vector3.Lerp(startPos, endPos, timer);
                CachedTransform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                CachedTransform.localScale = Vector3.Lerp(startScale, endScale, timer);
                
                if (timer>=1f)  break;
                
                yield return CachedWaitFrame;
            }

            gameObject.SetActive(false);
            // if (destroy)
            //     Destroy(gameObject);
           
        }

        #endregion
        
    }
}