using System;
using System.Collections.Generic;
using Card;
using Card.Display;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Money
{
    public interface ICardShopUI
    {
        void ShowUpgradeUI(CardUpgradeCommodity commodity);
    }

    public class CardShopUI : SerializedMonoBehaviour, ICardShopUI
    {
        [SerializeField] private ShopCard prefab;
        
        [SerializeField] private List<ShopCard> spawnedCardList = new List<ShopCard>();
        [SerializeField] private Transform spawnPos;
        
        [SerializeField] private CardUpgradePanel _cardUpgradeCommodityUI;

        
        private void Awake()
        {
            // ShopHandler.OnBuyCardUpgradeCommodity.AddListener((c) =>
            // {
            //     ShowCardCommodities();
            // });
        }

        [Button]
        public void ShowCardCommodities()
        {
            DestroyPreviousUI();

            var cardInfos = CardManager.Instance.GetAllCardInfos();
            Debug.Log($"prefab {prefab.name}");
            foreach (var cardInfo in cardInfos)
            {
                var card = Instantiate(prefab, spawnPos);
                spawnedCardList.Add(card);
                card.Init(cardInfo);
                card.SetData(new CardUpgradeCommodity(cardInfo), this);
            }
        }
        
        /// <summary>  
        /// Destroys the previously created UI elements.
        /// </summary>
        private void DestroyPreviousUI()
        {
            foreach (var cardBase in spawnedCardList)
            {
                Destroy(cardBase.gameObject);
            }

            spawnedCardList.Clear();
        }

        public void ShowUpgradeUI(CardUpgradeCommodity commodity)
        {
            _cardUpgradeCommodityUI.ShowPanel(commodity);
        }
    }
}