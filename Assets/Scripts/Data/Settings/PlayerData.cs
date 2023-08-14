using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Settings;
using NueGames.Relic;

namespace Data
{
    [Serializable]
    public class PlayerData
    {
        public int DrawCount;
        public int MaxMana;
        
        public int CurrentGold;
        public List<RelicClip> CurrentRelicList;
        
        public AllyHealthData AllyHealthData;
        
        public List<int> currentCards;

        #region 需要刪除

        public bool CanSelectCards;
        
        public bool IsFinalEncounter;
        
        
        #endregion

        public PlayerData()
        {
            CurrentRelicList = new List<RelicClip>();
            AllyHealthData = new AllyHealthData();
        }
        
        public PlayerData(GameplayData gameplayData)
        {
            DrawCount = gameplayData.DrawCount;
            MaxMana = gameplayData.MaxMana;
            CanSelectCards = true;
            CurrentGold = 0;
            IsFinalEncounter = false;
            CurrentRelicList = new List<RelicClip>();
            AllyHealthData = new AllyHealthData();
        }

        public void SetHealth(int newCurrentHealth, int newMaxHealth)
        {
            AllyHealthData.CurrentHealth = newCurrentHealth;
            AllyHealthData.MaxHealth = newMaxHealth;
        }

    }
}