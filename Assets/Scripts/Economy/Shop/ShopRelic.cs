using Economy.Shop.Data;
using Relic.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Economy.Shop
{
    public class ShopRelic : MonoBehaviour
    {
        private RelicUpgradeCommodity _commodity;
        private IRelicShopUI _relicShopUI;

        public UIRelic UIRelic;

        [Required]
        public GameObject unGainRelic;
        
        [Required]
        public GameObject mainUI;

        public RelicInfo RelicInfo;
        
        
        public void SetData(RelicUpgradeCommodity commodity, IRelicShopUI relicShopUI, RelicInfo relicInfo)
        {
            RelicInfo = relicInfo;
            _commodity = commodity;   
            _relicShopUI = relicShopUI;

            if (relicInfo.relicSaveInfo.HasGained)
            {
                UIRelic.OnRelicChose += OpenUpgradePanel;
            }
            
            UIRelic.Init(relicInfo);
            unGainRelic.SetActive(!relicInfo.relicSaveInfo.HasGained);
            mainUI.SetActive(relicInfo.relicSaveInfo.HasGained);
            
        }

        public void OpenUpgradePanel()
        {
            _relicShopUI.ShowUpgradeUI(_commodity);
        }
    }
}