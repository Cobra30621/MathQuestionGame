using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card.Data
{
    [Serializable]
    public class CardInfo
    {
        [ShowInInspector]
        public CardData CardData { get; private set; }

        public int Level => CardSaveInfo.Level;

        [ShowInInspector]
        public CardSaveInfo CardSaveInfo { get; private set; }

        public CardLevelInfo CardLevelInfo => cardLevelInfo;

        public string Description => CardLevelInfo.DesLang;
        public int ManaCost;

        [SerializeField] private CardLevelInfo cardLevelInfo;

        public CardSprite CardSprite => _cardSprite;
        
        [SerializeField] private CardSprite _cardSprite;
        
        public bool ExhaustAfterPlay => cardLevelInfo.ExhaustAfterPlay;
        
        
        public bool ExhaustIfNotPlay => false;
        
        
        
        public CardInfo(CardData cardData, CardSaveInfo cardSaveInfo, CardSprite cardSprite)
        {
            CardData = cardData;
            CardSaveInfo = cardSaveInfo;
            cardLevelInfo = cardData.GetLevelInfo(CardSaveInfo.Level);
            _cardSprite = cardSprite;
            
            ManaCost = cardLevelInfo.ManaCost;
        }

        public CardInfo(CardInfo cardInfo, int level)
        {
            CardData = cardInfo.CardData;
            _cardSprite = cardInfo.CardSprite;
            CardSaveInfo = new CardSaveInfo()
            {
                Level = level
            };
            cardLevelInfo = CardData.GetLevelInfo(CardSaveInfo.Level);
            
            ManaCost = cardLevelInfo.ManaCost;
        }


        public override string ToString()
        {
            return
                $"{cardLevelInfo.TitleLang} : {Description}" +
                $"等級 : {Level}, ID: {CardData.CardId}";
        }
    }
    
    [Serializable]
    public class CardSaveInfo
    {
        [LabelText("等級")] public int Level;
        [LabelText("已獲取過")] public bool HasGained;
    }
}