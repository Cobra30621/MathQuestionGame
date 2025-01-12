using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Combat;
using Effect.Parameters;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueTooltip.Core;
using Power;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card.Display
{
    public abstract class CardBase : MonoBehaviour
    {
        [SerializeField] protected Transform descriptionRoot;

        public CardInfo CardInfo => _cardInfo;
        
        protected CardInfo _cardInfo;
        public CardData CardData => _cardInfo.CardData;
        public CardLevelInfo CardLevelInfo => _cardInfo.CardLevelInfo;
        
        public ActionTargetType TargetChoose => CardLevelInfo.TargetChoose;
        
        public bool SpecifiedEnemyTarget => 
            CardLevelInfo.TargetChoose == ActionTargetType.SpecifiedEnemy;

        [SerializeField] protected CardDisplay _cardDisplay;

        protected Camera _camera;


        public virtual void Init(CardData cardData)
        {
            var cardInfo = CardManager.Instance.CreateCardInfo(cardData);
            
            Init(cardInfo);
        }

        
        
        public virtual void Init(CardInfo cardInfo)
        {
            _camera = Camera.main;
            
            _cardInfo = cardInfo;
            
            UpdateCardLevelInfo();
            UpdateCardDisplay();
            
            CardManager.Instance.CardInfoUpdated.AddListener(OnCardInfoUpdated);
        }
        
        private void OnCardInfoUpdated(List<CardInfo> cardInfos)
        {
            var cardInfo = cardInfos.FirstOrDefault(c => c.CardData.CardId ==  _cardInfo.CardData.CardId);
            Debug.Log(cardInfo);
            
            if (cardInfo!= null)
            {
                _cardInfo = cardInfo;
            
                UpdateCardLevelInfo();
                UpdateCardDisplay();
            }
        }
        
        public void UpdateCardDisplay()
        {
            _cardDisplay.SetCard(_cardInfo);
        }

        protected void UpdateCardLevelInfo()
        {
            if (!_cardInfo.CardData.IsDevelopCard)
            {
                CardLevelInfo.SkillInfos = CardManager.Instance.GetSkillInfos(CardLevelInfo.skillIDs);
            }
        }

        
        #region Pointer Events
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            ShowTooltipInfo();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            HideTooltipInfo();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            HideTooltipInfo();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            ShowTooltipInfo();
        }
        #endregion
        

        #region Tool Tip

        private bool NeedShowTooltip()
        {
            return _cardInfo.CardSaveInfo.HasGained;
        }
        
        
        protected void ShowTooltipInfo()
        {
            Debug.Log("Show Tooltip");
            var tooltipManager = TooltipManager.Instance;
            
            // 增加消耗的提示
            if (CardLevelInfo.ExhaustAfterPlay)
            {
                var specialKeyword = tooltipManager.SpecialKeywordData.SpecialKeywordBaseList.Find(
                    x=>x.SpecialKeyword == SpecialKeywords.Exhaust);
                ShowTooltipInfo(specialKeyword.GetContent(),specialKeyword.GetHeader(),descriptionRoot);
            }
            
            foreach (var powerType in GetActionsPowerTypes())
            {
                PowerData powerData = tooltipManager.PowersData.GetPowerData(powerType);
                Debug.Log($"powerData {powerData}");
                if (powerData != null)
                {
                    ShowTooltipInfo(powerData.GetContent(),powerData.GetHeader(),descriptionRoot);
                }
                    
            }
        }

        public virtual void ShowTooltipInfo(string content, string header = "", Transform tooltipStaticTransform = null,Camera cam = null, float delayShow =0)
        {
            TooltipManager.Instance.ShowTooltip(content,header,tooltipStaticTransform, cam,delayShow);
        }

        public virtual void HideTooltipInfo()
        {
            TooltipManager.Instance.HideTooltip();
        }
        
        private List<PowerName> GetActionsPowerTypes()
        {
            List<PowerName> powerTypes = new List<PowerName>();
            
            foreach (var skillInfo in CardLevelInfo.SkillInfos)
            {
                // 跟格擋相關效果
                if (PowerHelper.IsBlockRelatedEffect(skillInfo.EffectID))
                {
                    powerTypes.Add(PowerName.Block);
                } // 跟能力相關效果
                else if (PowerHelper.IsPowerRelatedEffect(skillInfo.EffectID))
                {
                    PowerName powerName = (PowerName)skillInfo.EffectParameterList[0];
                    powerTypes.Add(powerName);
                }
            }
            
            return powerTypes;
        }

        

        #endregion
    }
}