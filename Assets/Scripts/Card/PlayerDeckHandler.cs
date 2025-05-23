using System.Collections.Generic;
using Card.Data;
using Feedback;
using Log;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// 玩家本局遊戲的卡組
    /// </summary>
    public class PlayerDeckHandler : MonoBehaviour, IDataPersistence
    {
        [LabelText("玩家本局遊戲的卡組")] public List<CardData> CurrentDeck;

 
        /// <summary>
        /// 獲得卡片的反饋
        /// </summary>
        [Required] [SerializeField] private IFeedback gainCardFeedback;
        
        /// <summary>
        /// 丟棄卡片的反饋
        /// </summary>
        [Required] [SerializeField] private IFeedback throwCardFeedback;

        [Required] [SerializeField] private CardDataOverview cardDataOverview;
        
        
        /// <summary>
        /// 設定初始化卡牌
        /// </summary>
        /// <param name="cardDatas"></param>
        public void SetInitCard(List<CardData> cardDatas)
        {
            CurrentDeck = new List<CardData>();
            foreach (var cardData in cardDatas)
            {
                GainCardWithoutFeedback(cardData);
            }
        }
        
        /// <summary>
        /// 獲得卡牌
        /// </summary>
        /// <param name="cardData"></param>
        public void GainCard(CardData cardData)
        {
            GainCardWithoutFeedback(cardData);
            
            gainCardFeedback.Play();
        }

        /// <summary>
        /// 獲得卡牌
        /// </summary>
        /// <param name="cardData"></param>
        private void GainCardWithoutFeedback(CardData cardData)
        {
            CurrentDeck.Add(cardData);
            
            EventLogger.Instance.LogEvent(LogEventType.Card, $"獲得卡牌 - {cardData.name}, id:{cardData.CardId}");
            CardManager.Instance.cardLevelHandler.OnGainCard(cardData.CardId);
        }

        /// <summary>
        /// 丟棄卡牌
        /// </summary>
        /// <param name="cardData"></param>
        public void ThrowCard(CardData cardData)
        {
            CurrentDeck.Remove(cardData);
            
            EventLogger.Instance.LogEvent(LogEventType.Card, $"移除卡牌 - {cardData.name}, id:{cardData.CardId}");
            throwCardFeedback.Play();
            
        }

        #region 存檔、讀檔

        public void LoadData(GameData data)
        {
            CurrentDeck = new List<CardData>();

            foreach (var cardName in data.CardNames)
            {
                var cardData = cardDataOverview.FindUniqueId(cardName.Id);
                CurrentDeck.Add(cardData);
            }
        }

        public void SaveData(GameData data)
        {
            var cardNames = new List<CardName>();

            foreach (var cardData in CurrentDeck)
            {
                var cardName = cardDataOverview.GetCardName(cardData);
                cardNames.Add(cardName);
            }

            data.CardNames = cardNames;
        }

        #endregion
    }
}