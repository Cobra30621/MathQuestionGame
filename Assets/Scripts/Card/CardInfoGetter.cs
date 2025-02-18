using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Effect;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// 卡片資訊取得器
    /// </summary>
    public class CardInfoGetter : MonoBehaviour
    {
        
        /// <summary>
        /// 技能資料
        /// </summary>
        [Required] [SerializeField] private SkillData skillData;

        
        /// <summary>
        /// 取得單一職業所有卡牌
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public List<CardInfo> GetCardInfos(AllyClassType classType)
        {
            return GetAllCardInfos().Where(info => info.CardData.AllyClassType == classType).ToList();
        }

        /// <summary>
        /// 取得所有職業卡牌(排除通用牌)
        /// </summary>
        /// <returns></returns>
        public List<CardInfo> GetAllAllyClassCardInfos()
        {
            return GetAllCardInfos().Where(
                info => info.CardData.AllyClassType != AllyClassType.General).ToList();
        }
        
        /// <summary>
        /// 取得所有卡牌
        /// </summary>
        /// <returns></returns>
        public List<CardInfo> GetAllCardInfos()
        {
            var saveDeck = CardManager.Instance.saveDeck;
            var cardInfos = saveDeck.CardList.Select(CreateCardInfo).ToList();

            return cardInfos;
        }

        /// <summary>
        /// 創建卡牌資訊
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public List<CardInfo> CreateCardInfos(List<CardData> cardData)
        {
            var cardInfos = cardData.Select(CreateCardInfo).ToList();

            return cardInfos;
        }
        
        /// <summary>
        /// 創建卡牌資訊
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public CardInfo CreateCardInfo(CardData cardData)
        {
            // 取得存檔資料
            var saveInfo = CardManager.Instance.cardLevelHandler.GetSaveInfo(cardData.CardId);
            // 取得圖片
            var cardSprite = CardManager.Instance.characterCardSprites.GetSprite(cardData.AllyClassType);

            var cardInfo = new CardInfo(cardData, saveInfo, cardSprite);

            return cardInfo;
        }
        
        /// <summary>
        /// 使用 id 來獲得卡牌資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public bool GetCardDataWithId(string id, out CardData cardData)
        {
            // 尋找儲存的卡片
            foreach (var data in CardManager.Instance.saveDeck.CardList)
            {
                if (id == data.CardId)
                {
                    cardData = data;
                    return true;
                }
            }
            
            // 尋找其他的卡片
            foreach (var data in CardManager.Instance.otherCardDeck.CardList)
            {
                if (id == data.CardId)
                {
                    cardData = data;
                    return true;
                }
            }

            Debug.LogError("找不到卡片 " + id);
            cardData = null;
            return false;

        }
        
        
        /// <summary>
        /// 使用技能 id 取得技能資訊
        /// </summary>
        /// <param name="skillIds"></param>
        /// <returns></returns>
        public List<SkillInfo> GetSkillInfos(List<string> skillIds)
        {
            var skillInfos = new List<SkillInfo>();
            foreach (var skillId in skillIds)
            {
                var skillInfo = skillData.GetSkillInfo(skillId);
                if (skillInfo!= null)
                {
                    skillInfos.Add(skillInfo);
                }
            }
            
            return skillInfos;
        }

    }
}