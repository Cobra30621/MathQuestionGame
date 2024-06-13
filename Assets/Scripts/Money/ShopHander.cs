using System.Collections.Generic;
using Card;
using UnityEngine.Events;

namespace Money
{
    public class ShopHandler
    {
        public static UnityEvent<CardUpgradeCommodity> OnBuyCardUpgradeCommodity = 
            new UnityEvent<CardUpgradeCommodity>();


        public List<Commodity> GetAllCardCommodities()
        {
            var cardInfos = CardManager.Instance.GetAllCardInfos();
            var cardUpgradeCommodities = new List<Commodity>();
            
            foreach (var cardInfo in cardInfos)
            {
                var cardUpgradeCommodity = new CardUpgradeCommodity(cardInfo);
                cardUpgradeCommodities.Add(cardUpgradeCommodity);
            }

            return cardUpgradeCommodities;
        }
    }
}