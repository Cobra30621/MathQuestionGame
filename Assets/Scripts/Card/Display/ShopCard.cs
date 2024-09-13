using Card.Data;
using Coin;
using Money;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card.Display
{
    public class ShopCard : MonoBehaviour
    {
        [ShowInInspector]
        private CardUpgradeCommodity _commodity;
        private ICardShopUI _cardShopUI;

        public UICard UICard;

        [Required]
        public GameObject unGainCard;
        
        
        public void SetData(CardUpgradeCommodity commodity, ICardShopUI cardShopUI, CardInfo cardInfo)
        {
            _commodity = commodity;   
            _cardShopUI = cardShopUI;

            if (cardInfo.CardSaveInfo.HasGained)
            {
                UICard.OnCardChose += OpenUpgradePanel;
            }
            
            UICard.Init(cardInfo);
            unGainCard.SetActive(!cardInfo.CardSaveInfo.HasGained);
            
        }

        public void OpenUpgradePanel()
        {
            _cardShopUI.ShowUpgradeUI(_commodity);
        }
    }
}