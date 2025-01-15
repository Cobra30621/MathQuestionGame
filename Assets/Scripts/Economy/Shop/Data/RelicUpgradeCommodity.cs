using System.Collections.Generic;
using Log;
using Managers;
using Relic;
using Relic.Data;
using UnityEngine;

namespace Economy.Shop.Data
{
    public class RelicUpgradeCommodity : Commodity
    {
        [SerializeField]
        private RelicInfo _relicInfo;
        public RelicInfo RelicInfo => _relicInfo;


        public RelicUpgradeCommodity(RelicInfo relicInfo)
        {
            _relicInfo = relicInfo;
        }


        public Dictionary<CoinType, int> NeedCost()
        {
            return GameManager.Instance.RelicManager.UpgradeNeedCost();
        }

        public bool EnableBuy()
        {
            bool notMaxLevel = !_relicInfo.relicSaveInfo.IsMaxLevel();
            
            bool enoughMoney = CoinManager.Instance.EnoughCoin(NeedCost());
            
            return notMaxLevel && enoughMoney;
        }

        public void Buy()
        {
            CoinManager.Instance.Buy(NeedCost());
            
            GameManager.Instance.RelicManager.UpgradeRelic(_relicInfo.relicName);

            EventLogger.Instance.LogEvent(LogEventType.Economy, $"升級遺物: {_relicInfo}");
            ShopHandler.OnBuyRelicUpgradeCommodity.Invoke(this);
        }
    }
}