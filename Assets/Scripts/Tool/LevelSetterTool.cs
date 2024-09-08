using System;
using System.Collections.Generic;
using Card;
using Card.Data;
using NueGames.Collection;
using NueGames.Managers;
using NueGames.Relic;
using Relic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool
{
    
    
    public class LevelSetterTool : SerializedMonoBehaviour
    {
        [LabelText("遊戲開始時，設定等級資料")]
        public bool setDataWhenGameStarted;
        
        [TableList(AlwaysExpanded = true)]
        [LabelText("卡片等級")]
        public List<CardLevel> cardDataLevels;
        
        [TableList(AlwaysExpanded = true)]
        [LabelText("遺物等級")]
        public List<RelicInfo> relicInfos;
        
        private CardLevelHandler levelHandler => CardManager.Instance.CardLevelHandler;

        private RelicManager relicManager => GameManager.Instance.RelicManager;
        
        
        

        private void Start()
        {
            if (setDataWhenGameStarted)
            {
                SetRelicSaveInfo();
                SetCardLevel();
            }
        }
        
        [Button("創建等級清單")]
        public void GetCurrentCardLevel()
        {
            levelHandler.InitDictionary();
            cardDataLevels = new List<CardLevel>();

            foreach (var pair in levelHandler.cardLevels)
            {
                var cardData = levelHandler.SaveCard.GetCard(pair.Key);
                cardDataLevels.Add(new CardLevel(cardData, pair.Value.Level, pair.Value.HasGained));
            }

            relicManager.relicLevelHandler.InitRelicLevels();
            relicInfos = relicManager.GetAllRelicInfo();
        }


        [Button("設定卡片等級")]
        public void SetCardLevel()
        {
            var cardLevels = new Dictionary<string, CardSaveLevel>();
            foreach (var cardInfo in cardDataLevels)
            {
                var cardSaveLevel= new CardSaveLevel()
                {
                    Level = cardInfo.Level,
                    HasGained = cardInfo.HasGained
                };

                cardLevels[cardInfo.cardData.CardId] = cardSaveLevel;
            }

            levelHandler.cardLevels = cardLevels;
            
            CardManager.Instance.UpdateCardInfos();
        }

        [Button("設定遺物資訊")]
        public void SetRelicSaveInfo()
        {
            relicManager.SetRelicsInfo(relicInfos);
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

        [LabelText("已獲取過")]
        public bool HasGained;

        private int LevelRange => cardData.LevelInfos.Count - 1;
        
        
        public CardLevel(CardData cardData, int level, bool hasGained)
        {
            this.cardData = cardData;
            Level = level;
            HasGained = hasGained;
        }
    }
}