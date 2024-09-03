using System.Collections.Generic;
using Card;
using Card.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Coin
{
    public class CardUpgradeCommodity : Commodity
    {
        [SerializeField] private CardInfo _cardInfo;
        
        public CardInfo CardInfo => _cardInfo;

        public CardUpgradeCommodity(CardInfo cardInfo)
        {
            _cardInfo = cardInfo;
        }


        public Dictionary<CoinType, int> NeedCost()
        {
            return new Dictionary<CoinType, int>()
            {
                { CoinType.Money, _cardInfo.CardLevelInfo.UpgradeCost },
            };
        }

        public bool EnableBuy()
        {
            bool notMaxLevel = !_cardInfo.CardLevelInfo.MaxLevel;
            
            bool enoughMoney = CoinManager.Instance.EnoughCoin(NeedCost());
            
            return notMaxLevel && enoughMoney;
        }

        [Button]
        public void Buy()
        {
            CoinManager.Instance.Buy(NeedCost());

            string cardId = _cardInfo.CardData.CardId;
            CardManager.Instance.UpgradeCard(cardId);

            _cardInfo = CardManager.Instance.CreateCardInfo(_cardInfo.CardData);
            ShopHandler.OnBuyCardUpgradeCommodity.Invoke(this);
        }
    }
}