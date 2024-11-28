using System;
using System.Collections.Generic;
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

        public CardInfo(CardData cardData, CardSaveInfo cardSaveInfo)
        {
            CardData = cardData;
            CardSaveInfo = cardSaveInfo;
            cardLevelInfo = cardData.GetLevelInfo(CardSaveInfo.Level);
            
            ManaCost = cardLevelInfo.ManaCost;
            
        }

        public CardInfo(CardData cardData, int level)
        {
            CardData = cardData;
            CardSaveInfo = new CardSaveInfo()
            {
                Level = level
            };
            cardLevelInfo = cardData.GetLevelInfo(CardSaveInfo.Level);
            
            ManaCost = cardLevelInfo.ManaCost;
        }
        
    }
    
    [Serializable]
    public class CardSaveInfo
    {
        [LabelText("等級")] public int Level;
        [LabelText("已獲取過")] public bool HasGained;
    }
}