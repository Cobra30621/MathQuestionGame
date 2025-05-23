using System.Collections;
using System.Collections.Generic;
using Economy.Shop.Data;
using Managers;
using Relic;
using Relic.Data;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Economy.Shop
{
    public interface IRelicShopUI
    {
        void ShowUpgradeUI(RelicUpgradeCommodity commodity);
    }

    public class RelicShopUI : MonoBehaviour, IRelicShopUI
    {
        [SerializeField] private ShopRelic prefab;
        
        [SerializeField] private List<ShopRelic> spawnedRelicList = new List<ShopRelic>();
        [SerializeField] private Transform spawnPos;
        
        [SerializeField] private RelicUpgradePanel _relicUpgradeCommodityUI;

        
        /// <summary>
        /// 開啟指定寶物清單的商店
        /// </summary>
        /// <param name="relicInfos"></param>
        public void ShowCommodities()
        {
            List<RelicInfo> relicInfos = GameManager.Instance.RelicManager.GetAllRelicInfo();
            
            DestroyPreviousUI();

            foreach (var relicInfo in relicInfos)
            {
                var relic = Instantiate(prefab, spawnPos);
                spawnedRelicList.Add(relic);
                relic.SetData(new RelicUpgradeCommodity(relicInfo), this, relicInfo);
            }
        }
        
        
        
        
        /// <summary>  
        /// Destroys the previously created UI elements.
        /// </summary>
        private void DestroyPreviousUI()
        {
            foreach (var relicBase in spawnedRelicList)
            {
                Destroy(relicBase.gameObject);
            }

            spawnedRelicList.Clear();
        }

        public void ShowUpgradeUI(RelicUpgradeCommodity commodity)
        {
            _relicUpgradeCommodityUI.ShowPanel(commodity);
        }
    }
}