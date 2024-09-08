using Coin;
using Money.Data;
using UnityEngine.Events;

namespace Money
{
    public class ShopHandler
    {
        public static UnityEvent<CardUpgradeCommodity> OnBuyCardUpgradeCommodity = 
            new UnityEvent<CardUpgradeCommodity>();

        public static UnityEvent<RelicUpgradeCommodity> OnBuyRelicUpgradeCommodity = 
            new UnityEvent<RelicUpgradeCommodity>();


    }
}