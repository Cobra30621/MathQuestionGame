// File: /Users/cobra/Desktop/Unity/Develop/MathQuestionGame/Assets/Scripts/Relic/Display/ShopRelic.cs

using Economy.Shop.Data;
using Relic;
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

        public RelicInfo RelicInfo;
        
        
        public void SetData(RelicUpgradeCommodity commodity, IRelicShopUI relicShopUI, RelicInfo relicInfo)
        {
            RelicInfo = relicInfo;
            _commodity = commodity;   
            _relicShopUI = relicShopUI;

            if (relicInfo.relicSaveInfo.HasGained)
            {
                Debug.Log("Subscribe OnRelicChose");
                UIRelic.OnRelicChose += OpenUpgradePanel;
            }
            
            UIRelic.Init(relicInfo);
            unGainRelic.SetActive(!relicInfo.relicSaveInfo.HasGained);
            
        }

        public void OpenUpgradePanel()
        {
            Debug.Log(" OpenUpgradePanel");
            _relicShopUI.ShowUpgradeUI(_commodity);
        }
    }
}