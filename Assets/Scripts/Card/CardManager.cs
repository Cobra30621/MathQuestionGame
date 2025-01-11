using System.Collections.Generic;
using System.Linq;
using Action;
using Card.Data;
using Data;
using DataPersistence;
using Managers;
using NueGames.Enums;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Card
{
    public class CardManager : SerializedMonoBehaviour, IDataPersistence
    {
        public static CardManager Instance => GameManager.Instance.CardManager;
        
        [Required]
        [LabelText("會放入存檔的卡片")]
        [SerializeField] private DeckData saveDeck;

        [Required]
        [LabelText("其他卡片")]
        [SerializeField] private DeckData otherCardDeck;

        [Required] [LabelText("角色卡片圖片")] [SerializeField]
        private CharacterCardSprites characterCardSprites;
        
        
        [Required] [SerializeField] private ScriptableObjectFileHandler cardDataFileHandler;
        
        [Required]
        [SerializeField] private readonly CardLevelHandler _cardLevelHandler;

        [Required] [SerializeField] private SkillData skillData;
        
        public CardLevelHandler CardLevelHandler => _cardLevelHandler;

        public UnityEvent<List<CardInfo>> CardInfoUpdated;

        public List<CardData> CurrentCardsList;


        

        public void SetCurrenCardsList(List<CardData> cardData)
        {
            CurrentCardsList = cardData;
        }

        public void SetInitCard(List<CardData> cardDatas)
        {
            CurrentCardsList = new List<CardData>();
            foreach (var cardData in cardDatas)
            {
                GainCard(cardData);
            }
        }

        public List<string> GetCardGuid()
        {
            return cardDataFileHandler.DataToGuid(CurrentCardsList);
        }
        
        public void GainCard(CardData cardData)
        {
            CurrentCardsList.Add(cardData);
            
            _cardLevelHandler.OnGainCard(cardData.CardId);
        }

        public void ThrowCard(CardData cardData)
        {
            CurrentCardsList.Remove(cardData);
        }
        
        
        public void UpgradeCard(string cardId)
        {
            _cardLevelHandler.UpgradeCard(cardId);
            UpdateCardInfos();
        }

        public void UpdateCardInfos()
        {
            var cardInfos = GetAllCardInfos();
            
            CardInfoUpdated.Invoke(cardInfos);
        }

        public List<SkillInfo> GetSkillInfos(List<string> skillIds)
        {
            return skillIds.ConvertAll(id => skillData.GetSkillInfo(id));
        }
        
        public List<CardInfo> GetCardInfos(AllyClassType classType)
        {
            return GetAllCardInfos().Where(info => info.CardData.AllyClassType == classType).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<CardInfo> GetAllAllyClassCardInfos()
        {
            return GetAllCardInfos().Where(info => info.CardData.AllyClassType != AllyClassType.General).ToList();
        }
        

        public List<CardInfo> GetAllCardInfos()
        {
            var cardInfos = saveDeck.CardList.Select(CreateCardInfo).ToList();

            return cardInfos;
        }

        

        public List<CardInfo> CreateCardInfos(List<CardData> cardData)
        {
            var cardInfos = cardData.Select(CreateCardInfo).ToList();

            return cardInfos;
        }
        

        public CardInfo CreateCardInfo(CardData cardData)
        {
            var saveInfo = _cardLevelHandler.GetSaveInfo(cardData.CardId);
            var cardSprite = characterCardSprites.GetSprite(cardData.AllyClassType);

            var cardInfo = new CardInfo(cardData, saveInfo, cardSprite);

            return cardInfo;
        }

        public bool GetCardDataWithId(string id, out CardData cardData)
        {
            // 尋找儲存的卡片
            foreach (var data in saveDeck.CardList)
            {
                if (id == data.CardId)
                {
                    cardData = data;
                    return true;
                }
            }
            
            // 尋找其他的卡片
            foreach (var data in otherCardDeck.CardList)
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
        
        
        
        public void LoadData(GameData data)
        {
            SetCurrenCardsList(
                cardDataFileHandler.GuidToData<CardData>(data.CardDataGuids));
        }

        public void SaveData(GameData data)
        {
            data.CardDataGuids =  cardDataFileHandler.DataToGuid(
                CurrentCardsList);
        }
    }
}