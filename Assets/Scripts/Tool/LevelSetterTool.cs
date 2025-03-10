using System;
using System.Collections.Generic;
using Card;
using Card.Data;
using Managers;
using Relic;
using Relic.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool
{
    /// <summary>
    /// A tool for setting card and relic levels for testing purposes.
    /// </summary>
    public class LevelSetterTool : SerializedMonoBehaviour
    {
        [TableList(AlwaysExpanded = true)] [LabelText("卡片清單")]
        public List<CardLevel> cardDataLevels;

        [LabelText("遺物清單")]
        public Dictionary<RelicName, RelicSaveInfo> relicInfos;
        
        private CardLevelHandler cardLevelHandler => CardManager.Instance.cardLevelHandler;

        private RelicLevelHandler relicLevelHandler => GameManager.Instance.RelicManager.relicLevelHandler;
       

        /// <summary>
        /// Retrieves the current card and relic levels.
        /// </summary>
        [Button("創建存檔清單")]
        public void CreateSaveInfos()
        {
            cardLevelHandler.InitDictionary();
            cardDataLevels = new List<CardLevel>();

            foreach (var pair in cardLevelHandler.cardSaveInfos)
            {
                var cardData = cardLevelHandler.SaveCard.GetCard(pair.Key);
                cardDataLevels.Add(new CardLevel(cardData, pair.Value.Level, pair.Value.HasGained));
            }

            relicLevelHandler.InitDictionary();
            relicInfos = relicLevelHandler.relicSaveInfos;
        }

        /// <summary>
        /// Sets the card levels.
        /// </summary>
        [Button("設定卡片等級")]
        private void SetCardSaveInfo()
        {
            var cardLevels = new Dictionary<string, CardSaveInfo>();
            foreach (var cardInfo in cardDataLevels)
            {
                Debug.Log(cardInfo);
                var cardSaveLevel = new CardSaveInfo()
                {
                    Level = cardInfo.Level,
                    HasGained = cardInfo.HasGained
                };

                cardLevels[cardInfo.cardData.CardId] = cardSaveLevel;
            }

            cardLevelHandler.SetSaveInfo(cardLevels);
            CardManager.Instance.UpdateCardInfos();
        }

        /// <summary>
        /// Sets the relic save info.
        /// </summary>
        [Button("設定遺物等級")]
        private void SetRelicSaveInfo()
        {
            relicLevelHandler.SetSaveInfo(relicInfos);
        }
    }

    /// <summary>
    /// Represents a card level.
    /// </summary>
    public class CardLevel
    {
        [InlineEditor] [LabelText("Card")] public CardData cardData;

        [PropertyRange(0, "LevelRange")] [LabelText("等級")]
        public int Level;

        [LabelText("已經獲得")] public bool HasGained;

        private int LevelRange => cardData.LevelInfos.Count - 1;

        public CardLevel(CardData cardData, int level, bool hasGained)
        {
            this.cardData = cardData;
            Level = level;
            HasGained = hasGained;
        }
    }
}