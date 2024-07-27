using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Card
{
    public class CardManager : Singleton<CardManager>
    {
        [Required]
        [SerializeField] private DeckData saveDeck;
        [Required]
        [SerializeField] private readonly CardLevelHandler _cardLevelHandler;
        public CardLevelHandler CardLevelHandler => _cardLevelHandler;

        public UnityEvent<List<CardInfo>> CardInfoUpdated;


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

    }
}