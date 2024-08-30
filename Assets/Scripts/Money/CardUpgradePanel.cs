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
        public TextMeshProUGUI BeforeDiscription;
        public TextMeshProUGUI AfterDiscription;


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
            BeforeDiscription.text = cardInfo.Description;
            if (cardInfo.CardLevelInfo.MaxLevel)
            {
                after.gameObject.SetActive(false);
                upgrade.interactable = false;
                upgrade.gameObject.SetActive(false);
            }
            else
            {
                after.gameObject.SetActive(true);
                upgrade.interactable = true;
                var afterCardInfo = new CardInfo(cardInfo.CardData, cardInfo.Level + 1); 
                after.Init(afterCardInfo);
                AfterDiscription.text = afterCardInfo.Description;
            }
        }

        [Button]
        public void Upgrade()
        {
            _commodity.Buy();   
        }
    }
}