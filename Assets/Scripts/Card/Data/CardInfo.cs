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

        public int Level => CardSaveLevel.Level;

        [ShowInInspector]
        public CardSaveLevel CardSaveLevel { get; private set; }

        public CardLevelInfo CardLevelInfo => cardLevelInfo;

        public string Description => CardLevelInfo.DesLang;
        public int ManaCost;

        [SerializeField] private CardLevelInfo cardLevelInfo;

        public CardInfo(CardData cardData, CardSaveLevel cardSaveLevel)
        {
            CardData = cardData;
            CardSaveLevel = cardSaveLevel;
            cardLevelInfo = cardData.GetLevelInfo(CardSaveLevel.Level);
            
        }

        public CardInfo(CardData cardData, int level)
        {
            CardData = cardData;
            CardSaveLevel = new CardSaveLevel()
            {
                Level = level
            };
            cardLevelInfo = cardData.GetLevelInfo(CardSaveLevel.Level);
        }
        
    }
}