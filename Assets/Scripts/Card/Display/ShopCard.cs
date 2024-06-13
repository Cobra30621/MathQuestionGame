using Money;

namespace Card.Display
{
    public class ShopCard : CardBase
    {
        private CardUpgradeCommodity _commodity;
        private ICardShopUI _cardShopUI;

        public void SetData(CardUpgradeCommodity commodity, ICardShopUI cardShopUI)
        {
            _commodity = commodity;   
            _cardShopUI = cardShopUI;
        }

        public void OpenUpgradePanel()
        {
            _cardShopUI.ShowUpgradeUI(_commodity);
        }
    }
}