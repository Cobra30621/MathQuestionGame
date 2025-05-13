using System;
using Managers;

namespace Combat
{
    /// <summary>
    /// 管理戰鬥中的瑪娜
    /// </summary>
    public class ManaManager
    {
        
        /// <summary>
        /// 當獲得瑪娜時
        /// </summary>
        public static Action<int> OnGainMana;

        public int CurrentMana;
        
        
        /// <summary>
        /// 回合開始時，獲得瑪娜
        /// </summary>
        public void HandleAtTurnStartMana()
        {
            int maxMana = CombatManager.Instance.MaxMana();
            int gainValue = CombatCalculator.GetTurnStartManaValue(maxMana);
            
            // 每回合開始，將瑪娜歸零
            ReSetMana(); 
            // 獲取本回合瑪娜加成
            AddMana(gainValue);
        }
        

        /// <summary>
        /// 每回合開始，將瑪娜歸零
        /// </summary>
        public void ReSetMana()
        {
            CurrentMana = 0;
        }

        /// <summary>
        /// 獲得瑪娜
        /// </summary>
        /// <param name="mana"></param>
        public void AddMana(int mana)
        {
            CurrentMana += mana;

            if (mana != 0)
            {
                OnGainMana?.Invoke(mana);
            }
        }
        /// <summary>
        /// 設定瑪娜值
        /// <summary>
        public void SetMana(int mana)
        {
            CurrentMana = mana;
            OnGainMana?.Invoke(mana);
        }
        
    }
}