using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Data.Containers;
using Relic.Data;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Relic
{
    /// <summary>
    /// Handles the leveling and saving of relics in the game.
    /// </summary>
    public class RelicLevelHandler : SerializedMonoBehaviour, IPermanentDataPersistence
    {
        [ShowInInspector]
        public Dictionary<RelicName, RelicSaveInfo> relicSaveInfos { get; private set; }

        [SerializeField]
        public RelicsData relicsData;
    
        private bool haveInitDict = false;

        /// <summary>
        /// Initializes the relic level dictionary.
        /// </summary>
        public void InitDictionary()
        {
            haveInitDict = true;
            relicSaveInfos = new Dictionary<RelicName, RelicSaveInfo>();

            foreach (var pair in relicsData.RelicDict)
            {
                var relicId = pair.Key;
                relicSaveInfos[relicId] = new RelicSaveInfo { Level = 0, HasGained = false };
            }
        }

        /// <summary>
        /// Upgrades the level of the specified relic.
        /// </summary>
        /// <param name="name">The name of the relic.</param>
        /// <returns>The new level of the relic.</returns>
        public int UpgradeRelic(RelicName name)
        {
            CheckHaveInitDict();
            bool contain = relicSaveInfos.TryGetValue(name, out var relicSaveInfo);

            Debug.Log($"Upgrade relic {name} to {relicSaveInfo.Level + 1}");
            if (contain)
            {
                relicSaveInfo.Level++;
            }

            SaveManager.Instance.SavePermanentGame();

            return relicSaveInfo.Level;
        }

        /// <summary>
        /// Marks the specified relic as gained.
        /// </summary>
        /// <param name="relicName">The name of the relic.</param>
        public void OnGainRelic(RelicName relicName)
        {
            CheckHaveInitDict();
            relicSaveInfos[relicName].HasGained = true;
        }

        /// <summary>
        /// Retrieves the save info for the specified relic.
        /// </summary>
        /// <param name="relicName">The name of the relic.</param>
        /// <returns>The save info for the relic.</returns>
        public RelicSaveInfo GetRelicSaveInfo(RelicName relicName)
        {
            CheckHaveInitDict();
            return relicSaveInfos.TryGetValue(relicName, out var relicSaveInfo) ? 
                relicSaveInfo : new RelicSaveInfo();
        }

        public void SetSaveInfo(Dictionary<RelicName, RelicSaveInfo> saveInfos)
        {
            haveInitDict = true;
            relicSaveInfos = saveInfos;
        }

        /// <summary>
        /// Loads the save data from the specified game data.
        /// </summary>
        /// <param name="data">The game data.</param>
        public void LoadData(PermanentGameData data)
        {
            InitDictionary();

            var loadRelicInfo = data.relicInfo;
            if (loadRelicInfo != null)
            {
                // SaveDeck content takes precedence
                foreach (var relicId in relicSaveInfos.Keys.ToList())
                {
                    if (loadRelicInfo.ContainsKey(relicId))
                    {
                        relicSaveInfos[relicId] = loadRelicInfo[relicId];
                    }
                }
            }
        }

        /// <summary>
        /// Saves the current save data to the specified game data.
        /// </summary>
        /// <param name="data">The game data.</param>
        public void SaveData(PermanentGameData data)
        {
            data.relicInfo = relicSaveInfos;
        }
    
        private void CheckHaveInitDict()
        {
            if(haveInitDict)return;
            Debug.LogError($"{nameof(RelicLevelHandler)} have not init dictionary");
        }
    }

    [Serializable]
    public class RelicSaveInfo
    {
        public bool IsMaxLevel()
        {
            return Level > 0;
        }
    
        [LabelText("等級")]
        public int Level;
        [LabelText("已經獲得")]
        public bool HasGained;
    }
}