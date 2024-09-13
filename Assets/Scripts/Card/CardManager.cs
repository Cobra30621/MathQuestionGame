using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Data;
using DataPersistence;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Card
{
    public class CardManager : Singleton<CardManager>, IDataPersistence
    {
        [Required]
        [SerializeField] private DeckData saveDeck;
        [Required]
        [SerializeField] private readonly CardLevelHandler _cardLevelHandler;

        [Required] [SerializeField] private SkillData skillData;
        
        public CardLevelHandler CardLevelHandler => _cardLevelHandler;

        public UnityEvent<List<CardInfo>> CardInfoUpdated;

        public List<CardData> CurrentCardsList;


        [Required] [SerializeField] private ScriptableObjectFileHandler cardDataFileHandler;

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
            var level = _cardLevelHandler.GetCardLevel(cardData.CardId);

            var cardInfo = new CardInfo(cardData, level);

            return cardInfo;
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