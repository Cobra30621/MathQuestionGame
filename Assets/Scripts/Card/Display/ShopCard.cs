using Card.Data;
using Coin;
using Money;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Card.Display
{
    public class ShopCard : MonoBehaviour
    {
        [ShowInInspector]
        private CardUpgradeCommodity _commodity;
        private ICardShopUI _cardShopUI;

        public UICard UICard;

        [Required]
        public Image ungainCard;
        
        
        public void SetData(CardUpgradeCommodity commodity, ICardShopUI cardShopUI, CardInfo cardInfo)
        {
            _commodity = commodity;   
            _cardShopUI = cardShopUI;

            if (cardInfo.CardSaveInfo.HasGained)
            {
                UICard.OnCardChose += OpenUpgradePanel;
            }
            
            UICard.Init(cardInfo);

            ungainCard.sprite = cardInfo.CardSprite.ungainCardSprite;

            // 是否有得到卡牌過
            if (cardInfo.CardSaveInfo.HasGained)
            {
                ungainCard.gameObject.SetActive(false);
            }
            else
            {
                ungainCard.gameObject.SetActive(true);
                UICard.CloseAllDisplay();
            }
        }

        public void OpenUpgradePanel()
        {
            _cardShopUI.ShowUpgradeUI(_commodity);
        }
    }
}