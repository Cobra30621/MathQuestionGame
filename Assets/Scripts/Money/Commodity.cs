using System.Collections.Generic;

namespace Money
{
    public interface Commodity
    {


        Dictionary<CoinType, int> NeedCost();

        bool EnableBuy();

        void Buy();
    }

    public enum CoinType
    {
        Money,
        Stone
    }
}