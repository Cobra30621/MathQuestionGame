using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine.Events;


namespace Card
{
    /// <summary>
    /// 卡片管理器
    /// </summary>
    public class CardManager : SerializedMonoBehaviour
    {
        public static CardManager Instance => GameManager.Instance.CardManager;

        #region 資料
        [Required]
        [LabelText("會放入存檔的卡片")]
        public DeckData saveDeck;

        [Required]
        [LabelText("其他卡片")]
        public  DeckData otherCardDeck;

        [Required] [LabelText("角色卡片圖片")] 
        public CharacterCardSprites characterCardSprites;

        
        #endregion

        #region 處理器
        
        /// <summary>
        /// 卡片等級管理
        /// </summary>
        [Required]
        public CardLevelHandler cardLevelHandler;

        /// <summary>
        /// 玩家本局遊戲的卡組
        /// </summary>
        [Required] public PlayerDeckHandler playerDeckHandler;


        /// <summary>
        /// 提供卡牌資訊
        /// </summary>
        [Required] public CardInfoGetter cardInfoGetter;

        #endregion
        
        /// <summary>
        /// 事件:當卡片資訊更新
        /// </summary>
        public UnityEvent<List<CardInfo>> CardInfoUpdated;
        
        /// <summary>
        /// 玩家本局卡組
        /// </summary>
        public List<CardData> CurrentDeck => playerDeckHandler.CurrentDeck;



        #region 升級卡牌

        /// <summary>
        /// 升級卡牌
        /// </summary>
        /// <param name="cardId"></param>
        public void UpgradeCard(string cardId)
        {
            cardLevelHandler.UpgradeCard(cardId);
            UpdateCardInfos();
        }

        /// <summary>
        /// 更新卡牌資訊
        /// </summary>
        public void UpdateCardInfos()
        {
            var cardInfos =  cardInfoGetter.GetAllCardInfos();
            
            CardInfoUpdated.Invoke(cardInfos);
        }
        

        #endregion
        
    }
}