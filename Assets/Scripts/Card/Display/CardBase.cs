using System.Collections.Generic;
using System.Linq;
using Action;
using Card.Data;
using NSubstitute.Core;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueDeck.ThirdParty.NueTooltip.Core;
using NueGames.NueDeck.ThirdParty.NueTooltip.CursorSystem;
using NueGames.Power;
using NueTooltip.Core;
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
            
                UpdateCardDisplay();
            }
        }
        
        public void UpdateCardDisplay()
        {
            _cardDisplay.SetCard(_cardInfo);
        }

        
        #region Pointer Events
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
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
        
        
        protected virtual void ShowTooltipInfo()
        {
            if(!NeedShowTooltip()) return;
            if (!descriptionRoot) return;
            if (CardData.KeywordsList == null) return;
            
           
            var tooltipManager = TooltipManager.Instance;
            foreach (var cardDataSpecialKeyword in CardData.KeywordsList)
            {
                var specialKeyword = tooltipManager.SpecialKeywordData.SpecialKeywordBaseList.Find(x=>x.SpecialKeyword == cardDataSpecialKeyword);
                if (specialKeyword != null)
                    ShowTooltipInfo(specialKeyword.GetContent(),specialKeyword.GetHeader(),descriptionRoot);
            }
            
            foreach (var powerType in GetActionsPowerTypes())
            {
                PowerData powerData = tooltipManager.PowersData.GetPowerData(powerType);
                if(powerData != null)
                    ShowTooltipInfo(powerData.GetContent(),powerData.GetHeader(),descriptionRoot);
            }
        }

        public virtual void ShowTooltipInfo(string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.Default,Camera cam = null, float delayShow =0)
        {
            TooltipManager.Instance.ShowTooltip(content,header,tooltipStaticTransform,targetCursor,cam,delayShow);
        }

        public virtual void HideTooltipInfo()
        {
            TooltipManager.Instance.HideTooltip();
        }
        
        private List<PowerName> GetActionsPowerTypes()
        {
            List<PowerName> powerTypes = new List<PowerName>();
            
            foreach (var effectInfo in CardLevelInfo.EffectInfos)
            {
                if (effectInfo.EffectID == GameActionType.ApplyPower)
                {
                    PowerName powerName = (PowerName)effectInfo.EffectParameterList[0];
                    powerTypes.Add(powerName);
                }
            }
            return powerTypes;
        }

        

        #endregion
    }
}