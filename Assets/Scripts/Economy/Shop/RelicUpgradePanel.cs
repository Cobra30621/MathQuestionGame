using Economy.Shop.Data;
using Relic;
using Relic.Data;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.Shop
{
    /// <summary>
    /// Handles the display and functionality of the relic upgrade panel in the relic shop.
    /// </summary>
    public class RelicUpgradePanel : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private UIRelic before, after;
        [SerializeField] private Button upgrade;
        private RelicUpgradeCommodity _commodity;

        [Required]
        [SerializeField] private TextMeshProUGUI moneyText, stoneText, titleText, levelText;


        private void Awake()
        {
            // Add listener to the event that triggers when a relic upgrade commodity is bought
            ShopHandler.OnBuyRelicUpgradeCommodity.AddListener(ShowPanel);
        }


        /// <summary>
        /// Displays the relic upgrade panel with the relevant information for the given relic upgrade commodity.
        /// </summary>
        /// <param name="commodity">The relic upgrade commodity to display information for.</param>
        public void ShowPanel(RelicUpgradeCommodity commodity)
        {
            mainPanel.SetActive(true);
            
            _commodity = commodity;
            var relicInfo = commodity.RelicInfo;
            before.Init(relicInfo);

            // Check if the relic is at max level
            if (relicInfo.relicSaveInfo.IsMaxLevel())
            {
                after.gameObject.SetActive(false);
            }
            else
            {
                after.gameObject.SetActive(true);
                after.Init(new RelicInfo(
                    relicInfo.relicName, relicInfo.data, new RelicSaveInfo(){Level = 1}));
            }

            var needCost = _commodity.NeedCost();
            upgrade.interactable = !relicInfo.relicSaveInfo.IsMaxLevel()
                                   && commodity.EnableBuy();

            titleText.text = $"選擇進化 {relicInfo.data.Title}";
            levelText.text = $"等級 {relicInfo.relicSaveInfo.Level + 1}/{2}";
            
            moneyText.text = needCost.TryGetValue(CoinType.Money, out var value) ? 
                $"所需金幣: {value}" : $"所需金幣: 0";
            
            stoneText.text = needCost.TryGetValue(CoinType.Stone, out value) ? 
                $"所需寶石: {value}" : $"所需寶石: 0";
            
        }


        /// <summary>
        /// Handles the upgrade button click event. Calls the buy method on the relic upgrade commodity.
        /// </summary>
        [Button]
        public void Upgrade()
        {
            _commodity.Buy();   
        }
    }
}