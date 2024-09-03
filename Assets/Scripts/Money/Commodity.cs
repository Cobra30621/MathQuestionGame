using System.Collections.Generic;

namespace Coin
{
    public interface Commodity
    {


        Dictionary<CoinType, int> NeedCost();

        bool EnableBuy();

        void Buy();
    }

    
}