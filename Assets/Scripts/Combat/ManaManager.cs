using NueGames.Managers;
using System;
using System.Collections.Generic;
using NueGames.Enums;
using NueGames.Power;


namespace NueGames.Combat
{
    /// <summary>
    /// 管理戰鬥中的瑪娜
    /// </summary>
    public class ManaManager
    {
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        
        /// <summary>
        /// 當獲得瑪娜時
        /// </summary>
        public Action<int> OnGainMana;

        public int CurrentMana;
        
        
        /// <summary>
        /// 回合開始時，獲得瑪娜
        /// </summary>
        public void HandleAtTurnStartMana()
        {
            int gainValue = GameManager.PlayerData.MaxMana;
            var allyPowers = CombatManager.MainAlly.GetPowerDict();
            
            // 能力系統瑪娜加成
            foreach (var (key, value) in allyPowers)
            {
                gainValue = value.AtGainTurnStartMana(gainValue);
            }

            // 遺物系統瑪娜加成
            var relics = GameManager.PlayerData.CurrentRelicList;
            foreach (var relicClip in relics)
            {
                gainValue = relicClip.Relic.AtGainTurnStartMana(gainValue);
            }
            
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
    }
}