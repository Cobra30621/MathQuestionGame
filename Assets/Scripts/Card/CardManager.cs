using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card
{
    public class CardManager : Singleton<CardManager>
    {
        [Required]
        [SerializeField] private DeckData saveDeck;
        [Required]
        [SerializeField] private readonly CardLevelHandler _cardLevelHandler;

        public CardLevelHandler CardLevelHandler => _cardLevelHandler;
        

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