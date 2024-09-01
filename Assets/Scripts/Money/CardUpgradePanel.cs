using System;
using Card;
using Card.Data;
using Card.Display;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Money
{
    public class CardUpgradePanel : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private UICard before, after;
        [SerializeField] private Button upgrade;
        private CardUpgradeCommodity _commodity;

        [Required]
        [SerializeField] private TextMeshProUGUI moneyText, stoneText;


        private void Awake()
        {
            ShopHandler.OnBuyCardUpgradeCommodity.AddListener(ShowPanel);
        }


        public void ShowPanel(CardUpgradeCommodity commodity)
        {
            Debug.Log("Show Upgrade Panel");
            mainPanel.SetActive(true);
            
            _commodity = commodity;
            var cardInfo = commodity.CardInfo;
            before.Init(cardInfo);
            if (cardInfo.CardLevelInfo.MaxLevel)
            {
                after.gameObject.SetActive(false);
                upgrade.interactable = false;
            }
            else
            {
                after.gameObject.SetActive(true);
                upgrade.interactable = true;
                after.Init(new CardInfo(cardInfo.CardData, cardInfo.Level + 1));
            }

            var needCost = _commodity.NeedCost();

            moneyText.text = needCost.TryGetValue(CoinType.Money, out var value) ? 
                $"所需金幣: {value}" : $"所需金幣: 0";
            
            stoneText.text = needCost.TryGetValue(CoinType.Stone, out value) ? 
                $"所需寶石: {value}" : $"所需寶石: 0";
            
        }

        [Button]
        public void Upgrade()
        {
            _commodity.Buy();   
        }
    }
}