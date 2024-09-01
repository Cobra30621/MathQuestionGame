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
        public List<CardLevel> cardDataLevels;
        
        [TableList(AlwaysExpanded = true)]
        public List<RelicInfo> relicInfos;
        
        private CardLevelHandler levelHandler => CardManager.Instance.CardLevelHandler;

        private RelicManager relicManager => GameManager.Instance.RelicManager;
        
        
        

        private void Start()
        {
            if (setDataWhenGameStarted)
            {
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
                cardDataLevels.Add(new CardLevel(cardData, pair.Value));
            }

            relicManager.relicLevelHandler.InitRelicLevels();
            relicInfos = relicManager.GetAllRelicInfo();
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

        private int LevelRange => cardData.LevelInfos.Count - 1;
        
        
        public CardLevel(CardData cardData, int level)
        {
            this.cardData = cardData;
            Level = level;
        }
    }
}