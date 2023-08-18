using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Settings;
using NueGames.Relic;

namespace Data
{
    /// <summary>
    /// 玩家資料(存檔用)
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public int DrawCount;
        public int MaxMana;
        public int CurrentGold;
        
        public AllyHealthData AllyHealthData;
        
        public string AllyDataGuid;
        
        /// <summary>
        /// 卡牌資料
        /// </summary>
        public List<string> CardDataGuids;
        
        /// <summary>
        /// 遺物資料
        /// </summary>
        public List<RelicName> Relics;

        public PlayerData()
        {
            AllyHealthData = new AllyHealthData();
        }
        
        public PlayerData(GameplayData gameplayData)
        {
            DrawCount = gameplayData.DrawCount;
            MaxMana = gameplayData.MaxMana;
            CurrentGold = 0;
            AllyHealthData = new AllyHealthData();
        }

        public void SetHealth(int newCurrentHealth, int newMaxHealth)
        {
            AllyHealthData.CurrentHealth = newCurrentHealth;
            AllyHealthData.MaxHealth = newMaxHealth;
        }

    }
}