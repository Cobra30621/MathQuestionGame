using System.Collections.Generic;
using Card.Data;
using Log;
using Save;
using Save.Data;
using Save.FileHandler;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// 玩家本局遊戲的卡組
    /// </summary>
    public class PlayerDeckHandler : MonoBehaviour, IDataPersistence
    {
        [LabelText("玩家本局遊戲的卡組")]
        public List<CardData> CurrentDeck;
        
        /// <summary>
        /// 獲得 CardData ScriptableObject 的處理器
        /// </summary>
        [Required] public ScriptableObjectFileHandler cardDataFileHandler;
        
        
        
        /// <summary>
        /// 設定初始化卡牌
        /// </summary>
        /// <param name="cardDatas"></param>
        public void SetInitCard(List<CardData> cardDatas)
        {
            CurrentDeck = new List<CardData>();
            foreach (var cardData in cardDatas)
            {
                GainCard(cardData);
            }
        }
        
        /// <summary>
        /// 獲得卡牌
        /// </summary>
        /// <param name="cardData"></param>
        public void GainCard(CardData cardData)
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
        }

        #region 存檔、讀檔

        public void LoadData(GameData data)
        {
            CurrentDeck = 
                cardDataFileHandler.GuidToData<CardData>(data.CardDataGuids);
        }

        public void SaveData(GameData data)
        {
            data.CardDataGuids =  cardDataFileHandler.DataToGuid(
                CurrentDeck);
        }

        #endregion
    }
}