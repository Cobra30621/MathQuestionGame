using Economy.Shop.Data;
using UnityEngine.Events;

namespace Economy.Shop
{
    public class ShopHandler
    {
        public static UnityEvent<CardUpgradeCommodity> OnBuyCardUpgradeCommodity = 
            new UnityEvent<CardUpgradeCommodity>();

        public static UnityEvent<RelicUpgradeCommodity> OnBuyRelicUpgradeCommodity = 
            new UnityEvent<RelicUpgradeCommodity>();


    }
}