using System;
using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using CardAction;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Utils;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.NueDeck.ThirdParty.NueTooltip.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using NueGames.Combat;
using NueGames.Parameters;
using NueGames.Power;
using Sirenix.OdinInspector;

namespace NueGames.Card
{
    public class CardBase : SerializedMonoBehaviour,I2DTooltipTarget, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Base References")]
        [SerializeField] protected Transform descriptionRoot;
        
        [SerializeField] protected Dictionary<RarityType, CardUI> cardUIDictionary;
        protected CardUI currentCard;


        #region Cache
        public CardData CardData { get; private set; }
        public bool IsInactive { get; protected set; }
        protected Transform CachedTransform { get; set; }
        protected WaitForEndOfFrame CachedWaitFrame { get; set; }
        public bool IsPlayable { get; protected set; } = true;
        public int ManaCost { get; protected set;}

        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        public bool IsExhausted { get; private set; }
        

        #endregion
        
        #region Setup
        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
        }

        public virtual void SetCard(CardData cardData,bool isPlayable = true)
        {
            CardData = cardData;
            IsPlayable = isPlayable;
            
            foreach (var pair in cardUIDictionary)
            {
                pair.Value.gameObject.SetActive(false);
            }
            currentCard = cardUIDictionary[CardData.Rarity];
            ManaCost = cardData.ManaCost;
            currentCard.gameObject.SetActive(true);
            UpdateCardText();
        }
        
        #endregion


        
        #region Card Methods
        // Math Action

        public bool ActionTargetIsSingleEnemy()
        {
            return CardData.ActionTargetType == ActionTargetType.Enemy;
        }
        
        public virtual void Use(List<CharacterBase> targetList)
        {
            if (!IsPlayable) return;

            HideTooltipInfo(TooltipManager.Instance);
            
            SpendMana( ManaCost);

            ActionSource actionSource = new ActionSource()
            {
                SourceType = SourceType.Card,
                SourceCard = this,
                SourceCharacter = CombatManager.MainAlly
            };
            
            CardData.CardAction.SetValue(this, targetList);
            CardData.CardAction.DoAction();
            
            CollectionManager.OnCardPlayed(this);
        }
        
        public virtual void Discard()
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            CollectionManager.OnCardDiscarded(this);
            StartCoroutine(DiscardRoutine());
        }
        
        public virtual void Exhaust(bool destroy = true)
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            IsExhausted = true;
            CollectionManager.OnCardExhausted(this);
            StartCoroutine(ExhaustRoutine(destroy));
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
            currentCard.SetPlayable(isInactive);
        }
        
        public virtual void UpdateCardText()
        {
            CardData.UpdateDescription();
            currentCard.UpdateUI(CardData, ManaCost);
            
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

            UpdateCardText();
        }

        /// <summary>
        /// 設置魔力花費
        /// </summary>
        /// <param name="cost"></param>
        public void SetManaCost(int cost)
        {
            ManaCost = cost;
            UpdateCardText();
        }
        

        #endregion
        
        
        #region Routines
        protected virtual IEnumerator DiscardRoutine(bool destroy = true)
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

            if (destroy)
                Destroy(gameObject);
           
        }
        
        protected virtual IEnumerator ExhaustRoutine(bool destroy = true)
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

            if (destroy)
                Destroy(gameObject);
           
        }

        #endregion

        #region Pointer Events
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo(TooltipManager.Instance);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            HideTooltipInfo(TooltipManager.Instance);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }
        #endregion

        #region Tooltip
        protected virtual void ShowTooltipInfo()
        {
            if (!descriptionRoot) return;
            if (CardData.KeywordsList == null) return;
           
            var tooltipManager = TooltipManager.Instance;
            Debug.Log($"CardData.KeywordsList{CardData.KeywordsList.Count}");
            foreach (var cardDataSpecialKeyword in CardData.KeywordsList)
            {
                var specialKeyword = tooltipManager.SpecialKeywordData.SpecialKeywordBaseList.Find(x=>x.SpecialKeyword == cardDataSpecialKeyword);
                if (specialKeyword != null)
                    ShowTooltipInfo(tooltipManager,specialKeyword.GetContent(),specialKeyword.GetHeader(),descriptionRoot,CursorType.Default,CollectionManager ? CollectionManager.HandController.cam : Camera.main);
            }
            
            Debug.Log($"GetActionsPowerTypes(){GetActionsPowerTypes()}");
            foreach (var powerType in GetActionsPowerTypes())
            {
                PowerData powerData = tooltipManager.PowersData.GetPowerData(powerType);
                if(powerData != null)
                    ShowTooltipInfo(tooltipManager,powerData.GetContent(),powerData.GetHeader(),descriptionRoot,CursorType.Default,CollectionManager ? CollectionManager.HandController.cam : Camera.main);
            }
        }

        public virtual void ShowTooltipInfo(TooltipManager tooltipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.Default,Camera cam = null, float delayShow =0)
        {
            tooltipManager.ShowTooltip(content,header,tooltipStaticTransform,targetCursor,cam,delayShow);
        }

        public virtual void HideTooltipInfo(TooltipManager tooltipManager)
        {
            tooltipManager.HideTooltip();
        }
        
        private List<PowerName> GetActionsPowerTypes()
        {
            List<PowerName> powerTypes = new List<PowerName>();
            
            // TODO GetActionsPowerTypes()
            // foreach (var cardActionData in CardData.CardActionDataList)
            // {
            //     if (cardActionData.actionName == ActionName.ApplyPower)
            //     {
            //         powerTypes.Add(cardActionData.powerName);
            //     }
            // }
            return powerTypes;
        }

        #endregion
       
    }
}