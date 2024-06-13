using Card;
using Card.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Money
{
    public class CardUpgradeCommodity : Commodity
    {
        [SerializeField] private CardInfo _cardInfo;
        
        public CardInfo CardInfo => _cardInfo;

        public CardUpgradeCommodity(CardInfo cardInfo)
        {
            _cardInfo = cardInfo;
        }
        
        
        public int NeedCost()
        {
            return _cardInfo.CardLevelInfo.UpgradeCost;
        }

        public bool EnableBuy()
        {
            bool notMaxLevel = !_cardInfo.CardLevelInfo.MaxLevel;
            bool enoughMoney = MoneyManager.Instance.EnoughMoney(NeedCost());
            
            return notMaxLevel && enoughMoney;
        }

        [Button]
        public void Buy()
        {
            int cost = NeedCost();
            MoneyManager.Instance.Buy(cost);

            var cardLevelHandler = CardManager.Instance.CardLevelHandler;
            string cardId = _cardInfo.CardData.CardId;
            cardLevelHandler.UpgradeCard(cardId);

            _cardInfo = CardManager.Instance.CreateCardInfo(_cardInfo.CardData);
            ShopHandler.OnBuyCardUpgradeCommodity.Invoke(this);
        }
    }
}