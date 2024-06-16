using Card.Data;
using Money;
using UnityEngine;

namespace Card.Display
{
    public class ShopCard : MonoBehaviour
    {
        private CardUpgradeCommodity _commodity;
        private ICardShopUI _cardShopUI;

        public UICard UICard;
        
        public void SetData(CardUpgradeCommodity commodity, ICardShopUI cardShopUI, CardInfo cardInfo)
        {
            _commodity = commodity;   
            _cardShopUI = cardShopUI;
            UICard.Init(cardInfo);
            
            UICard.OnCardChose += OpenUpgradePanel;
        }

        public void OpenUpgradePanel()
        {
            _cardShopUI.ShowUpgradeUI(_commodity);
        }
    }
}