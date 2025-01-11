using System.Collections.Generic;
using Managers;
using Save;
using Save.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Economy
{
    public enum CoinType
    {
        Money,
        Stone
    }

    /// <summary>
    /// Manages the player's money and stone coins.
    /// </summary>
    public class CoinManager : MonoBehaviour, IPermanentDataPersistence
    {
        public static CoinManager Instance => GameManager.Instance.CoinManager;
        
        
        [SerializeField] private int money;
        [SerializeField] private int stone;

        public int Money => money;
        public int Stone => stone;

        /// <summary>
        /// Event invoked when the coin amount changes.
        /// </summary>
        public static UnityEvent<int, CoinType> OnCoinChanged = new UnityEvent<int, CoinType>();

        

        /// <summary>
        /// Adds the specified amount of the given coin type to the player's total.
        /// </summary>
        public void AddCoin(int add, CoinType type)
        {
            switch (type)
            {
                case CoinType.Money:
                    SetMoney(money + add);
                    break;
                case CoinType.Stone:
                    SetStone(stone + add);
                    break;
            }
        }

        /// <summary>
        /// Removes the specified amounts of the given coin types from the player's total.
        /// </summary>
        public void Buy(Dictionary<CoinType, int> removeCoins)
        {
            foreach (var (type, remove) in removeCoins)
            {
                switch (type)
                {
                    case CoinType.Money:
                        SetMoney(money - remove);
                        break;
                    case CoinType.Stone:
                        SetStone(stone - remove);
                        break;
                }
            }
        }

        /// <summary>
        /// Sets the player's money amount and invokes the OnCoinChanged event.
        /// </summary>
        public void SetMoney(int set)
        {
            money = set;
            OnCoinChanged.Invoke(money, CoinType.Money);
        }

        /// <summary>
        /// Sets the player's stone amount and invokes the OnCoinChanged event.
        /// </summary>
        public void SetStone(int set)
        {
            stone = set;
            OnCoinChanged.Invoke(stone, CoinType.Stone);
        }

        /// <summary>
        /// Checks if the player has enough of the specified coin types.
        /// </summary>
        public bool EnoughCoin(Dictionary<CoinType, int> requiredCoins)
        {
            foreach (var (type, required) in requiredCoins)
            {
                switch (type)
                {
                    case CoinType.Money:
                        if (money < required) return false;
                        break;
                    case CoinType.Stone:
                        if (stone < required) return false;
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Loads the player's coin data from the given PermanentGameData object.
        /// </summary>
        public void LoadData(PermanentGameData data)
        {
            money = data.money;
            stone = data.stone;
        }

        /// <summary>
        /// Saves the player's coin data to the given PermanentGameData object.
        /// </summary>
        public void SaveData(PermanentGameData data)
        {
            data.money = money;
            data.stone = stone;
        }
    }
}