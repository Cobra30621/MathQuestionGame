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
        [ShowInInspector]
        public int Level { get; private set; }

        public CardLevelInfo CardLevelInfo => cardLevelInfo;

        public string Description => CardLevelInfo.DesLang;
        public int ManaCost;

        [SerializeField] private CardLevelInfo cardLevelInfo;

        public CardInfo(CardData cardData, int level)
        {
            CardData = cardData;
            Level = level;
            cardLevelInfo = cardData.GetLevelInfo(level);
            
        }
        
    }
}