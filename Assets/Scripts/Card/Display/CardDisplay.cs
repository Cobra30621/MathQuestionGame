using System.Collections.Generic;
using Card.Data;
using NueGames.Card;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card.Display
{
    public class CardDisplay : SerializedMonoBehaviour
    {
        [SerializeField] protected Dictionary<RarityType, SingleCardDisplay> cardUIDictionary;
        protected SingleCardDisplay CurrentSingleCard;

        [SerializeField] private CardInfo _cardInfo;

        public void SetCard(CardInfo cardInfo)
        {
            _cardInfo = cardInfo;

            var rarity = cardInfo.CardData.Rarity;
            UpdateRarity(rarity);
            
            CurrentSingleCard.UpdateUI(cardInfo);
        }

        public void SetPlayable(bool playable)
        {
            CurrentSingleCard.SetPlayable(playable);
        }
        

        private void UpdateRarity(RarityType rarityType)
        {
            CloseAllDisplay();
            
            CurrentSingleCard = cardUIDictionary[rarityType];
            CurrentSingleCard.gameObject.SetActive(true);
        }

        public void CloseAllDisplay()
        {
            foreach (var pair in cardUIDictionary)
            {
                pair.Value.gameObject.SetActive(false);
            }
        }
    }
}