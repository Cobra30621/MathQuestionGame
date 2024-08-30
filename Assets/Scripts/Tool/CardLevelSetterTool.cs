using System;
using System.Collections.Generic;
using Card;
using Card.Data;
using NueGames.Collection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool
{
    
    
    public class CardLevelSetterTool : SerializedMonoBehaviour
    {
        [LabelText("遊戲開始時，設定等級資料")]
        public bool setDataWhenGameStarted;
        
        [TableList(AlwaysExpanded = true)]
        public List<CardLevel> cardDataLevels;

        private CardLevelHandler levelHandler => CardManager.Instance.CardLevelHandler;
        

        private void Start()
        {
            if (setDataWhenGameStarted)
            {
                SetCardLevel();
            }
        }


        [Button("創建卡片等級清單")]
        public void GetCurrentCardLevel()
        {
            levelHandler.InitDictionary();
            cardDataLevels = new List<CardLevel>();

            foreach (var pair in levelHandler.cardLevels)
            {
                var cardData = levelHandler.SaveCard.GetCard(pair.Key);
                cardDataLevels.Add(new CardLevel(cardData, pair.Value));
            }

        }


        [Button("設定卡片等級")]
        public void SetCardLevel()
        {
            var cardLevels = new Dictionary<string, int>();
            foreach (var cardInfo in cardDataLevels)
            {
                cardLevels[cardInfo.cardData.CardId] = cardInfo.Level;
            }

            levelHandler.cardLevels = cardLevels;
            
            CardManager.Instance.UpdateCardInfos();
        }

    }
    
    public class CardLevel
    {
        [InlineEditor]
        [LabelText("卡片")]
        public CardData cardData;

        [PropertyRange(0, "LevelRange")]
        [LabelText("等級")]
        public int Level;

        private int LevelRange => cardData.LevelInfos.Count - 1;
        
        
        public CardLevel(CardData cardData, int level)
        {
            this.cardData = cardData;
            Level = level;
        }
    }
}