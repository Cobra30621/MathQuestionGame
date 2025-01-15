using System.Collections.Generic;
using System.Linq;
using Card;
using Card.Data;
using Card.Display;
using Economy.Shop.Data;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.Shop
{
    public interface ICardShopUI
    {
        void ShowUpgradeUI(CardUpgradeCommodity commodity);
    }

    public class CardShopUI : MonoBehaviour, ICardShopUI
    {
        [SerializeField] private ShopCard prefab;
        
        [SerializeField] private List<ShopCard> spawnedCardList = new List<ShopCard>();
        [SerializeField] private Transform spawnPos;
        
        [SerializeField] private CardUpgradePanel _cardUpgradeCommodityUI;

        [Required]
        [SerializeField] private AllyClassToggle togglePrefab;
        [Required]
        [SerializeField] private Transform toogleSpawnPos;

        [Required] [SerializeField] private ToggleGroup _toggleGroup;

        private void Awake()
        {
            BuildToggleList();
        }

        
        private List<(AllyClassType, string)> toggleList = new List<(AllyClassType, string)>()
        {
            (AllyClassType.Knight, "騎士"),
            (AllyClassType.Hunter, "獵人"),
            (AllyClassType.Mage, "法師"),
            (AllyClassType.Thief, "盜賊"),
        };
        
        
        private void BuildToggleList()
        {
            // 全部
            var allToggle = Instantiate(togglePrefab, toogleSpawnPos);
            allToggle.toggle.onValueChanged.AddListener((ison) =>
            {
                if(ison)
                    ShowAllyClassCardCommodities();
            });
            allToggle.info.text = "全部";
            allToggle.toggle.group = _toggleGroup;
            
            // 其他職業
            foreach (var (allyClassType, info) in toggleList)
            {
                var toggle = Instantiate(togglePrefab, toogleSpawnPos);
                toggle.toggle.onValueChanged.AddListener((ison) =>
                {
                    if (ison)
                        ShowCardCommodities(allyClassType);
                });
                toggle.info.text = info;
                toggle.toggle.group = _toggleGroup;
            }
        }

        /// <summary>
        /// 顯示通用卡商店
        /// </summary>
        public void ShowGeneralCardCommodities()
        {
            ShowCardCommodities(AllyClassType.General);
            
            toogleSpawnPos.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// 顯示職業卡商店
        /// </summary>
        public void ShowAllyClassCardCommodities()
        {
            toogleSpawnPos.gameObject.SetActive(true);
            
            var cardInfos = CardManager.Instance.GetAllAllyClassCardInfos();
            ShowCommodities(cardInfos);
        }
        

        /// <summary>
        /// 顯示特定職業商店
        /// </summary>
        /// <param name="classType"></param>
        [Button]
        public void ShowCardCommodities(AllyClassType classType)
        {
            var cardInfos = CardManager.Instance.GetCardInfos(classType);
            
            ShowCommodities(cardInfos);
        }
        
        /// <summary>
        /// 開啟指定卡片清單的商店
        /// </summary>
        /// <param name="cardInfos"></param>
        private void ShowCommodities(List<CardInfo> cardInfos)
        {
            DestroyPreviousUI();
            // 依據職業排序
            var sortedCardInfos = cardInfos.OrderBy(x => x.CardData.AllyClassType).ToList();
            
            foreach (var cardInfo in sortedCardInfos)
            {
                var card = Instantiate(prefab, spawnPos);
                spawnedCardList.Add(card);
                card.SetData(new CardUpgradeCommodity(cardInfo), this, cardInfo);
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