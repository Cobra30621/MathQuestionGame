using System.Collections.Generic;
using Coin;
using Managers;
using NueGames.Managers;
using Relic;
using UnityEngine;

namespace Money.Data
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

            ShopHandler.OnBuyRelicUpgradeCommodity.Invoke(this);
        }
    }
}