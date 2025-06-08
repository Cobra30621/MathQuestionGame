using System.Collections.Generic;
using Log;
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
        public static CoinManager Instance  => GameManager.Instance != null ? GameManager.Instance.CoinManager : null; 

        [SerializeField] private CoinFeedback feedback;
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
            int afterAdd = -1;
            switch (type)
            {
                case CoinType.Money:
                    afterAdd = money + add;
                    SetMoney(afterAdd);
                    break;
                case CoinType.Stone:
                    afterAdd = stone + add;
                    SetStone(afterAdd);
                    break;
            }

            if (add > 0)
            {
                feedback.PlayGainCoin();
            }
            EventLogger.Instance.LogEvent(LogEventType.Economy, $"獲得 {type} - {add}", $"獲得後數量 {afterAdd}");
            SaveManager.Instance.SavePermanentGame(); 
        }

        /// <summary>
        /// Removes the specified amounts of the given coin types from the player's total.
        /// </summary>
        public void Buy(Dictionary<CoinType, int> removeCoins)
        {
            string message = "";
            foreach (var (type, remove) in removeCoins)
            {
                switch (type)
                {
                    case CoinType.Money:
                        message += $"{type} 減少 {remove}, 剩下 {money - remove}\n";
                        SetMoney(money - remove);
                        break;
                    case CoinType.Stone:
                        message += $"{type} 減少 {remove}, 剩下 {stone - remove}\n";
                        SetStone(stone - remove);
                        break;
                }
            }
            
            EventLogger.Instance.LogEvent(LogEventType.Economy, $"購買", message);
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