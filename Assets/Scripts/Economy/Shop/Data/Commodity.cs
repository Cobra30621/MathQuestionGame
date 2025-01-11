using System.Collections.Generic;

namespace Economy.Shop.Data
{
    /// <summary>
    /// Represents an item or commodity that can be bought with in-game currency.
    /// </summary>
    public interface Commodity
    {
        /// <summary>
        /// Calculates the cost of buying the commodity in each available currency type.
        /// </summary>
        /// <returns>A dictionary mapping currency types to their respective costs.</returns>
        Dictionary<CoinType, int> NeedCost();

        /// <summary>
        /// Determines whether the commodity can currently be bought.
        /// </summary>
        /// <returns>True if the commodity can be bought, false otherwise.</returns>
        bool EnableBuy();

        /// <summary>
        /// Processes the purchase of the commodity, deducting the required currency from the player's total.
        /// </summary>
        void Buy();
    }
}