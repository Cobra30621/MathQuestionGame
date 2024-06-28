
using Data;
using UnityEngine;
using UnityEngine.Events;

namespace Money
{
    public class MoneyManager : Singleton<MoneyManager>, IDataPersistence
    {
        [SerializeField] private int money;
        public int Money => money;

        public static UnityEvent<int> OnMoneyChanged = new UnityEvent<int>();
        
        
        public void AddMoney(int add)
        {
            money += add;
            SetMoney(money + add);
        }
        
        public void Buy(int remove)
        {
            SetMoney(money - remove);
        }
        
        public void SetMoney(int set)
        {
            money = set;
            OnMoneyChanged.Invoke(money);
        }
        
        public bool EnoughMoney(int need)
        {
            return money >= need;
        }

        public void LoadData(GameData data)
        {
            money = data.Money;
        }

        public void SaveData(GameData data)
        {
            data.Money = money;
        }
    }
}