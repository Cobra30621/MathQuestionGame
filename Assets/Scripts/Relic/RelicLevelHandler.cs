using System.Collections.Generic;
using System.Linq;
using Data;
using NueGames.Data.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Relic
{
    public class RelicLevelHandler : SerializedMonoBehaviour, IDataPersistence
    {
        [SerializeField]
        private Dictionary<string, int> relicLevels;

        [SerializeField]
        private Dictionary<string, bool> haveGainedRelic;
        
        [SerializeField]
        public RelicsData relicsData;

        public void InitRelicLevels()
        {
            relicLevels = new Dictionary<string, int>();
            haveGainedRelic = new Dictionary<string, bool>();
            
            foreach (var relicData in relicsData.RelicList)
            {
                var relicId = relicData.RelicName.ToString();
                relicLevels[relicId] = 0;
                haveGainedRelic[relicId] = false;
            }
        }

        public int UpgradeRelic(RelicName name)
        {
            bool contain = relicLevels.TryGetValue(name.ToString(), out var relicLevel);

            Debug.Log($"Upgrade relic {name} to {relicLevel + 1}");
            if (contain)
            {
                relicLevels[name.ToString()]++;
            }

            SaveManager.Instance.SaveGame();

            return relicLevels[name.ToString()];
        }

        public void OnGainRelic(RelicName relicName)
        {
            haveGainedRelic[relicName.ToString()] = true;
        }

        public int GetRelicLevel(RelicName name)
        {
            return relicLevels.TryGetValue(name.ToString(), out var relicLevel) ? relicLevel : 0;
        }
        
        public bool HasGainRelic(RelicName relicName)
        {
            return haveGainedRelic.TryGetValue(relicName.ToString(), out var hasGain) && hasGain;
        }

        public void LoadData(GameData data)
        {
            InitRelicLevels();

            var loadRelicLevels = data.relicLevels;
            var loadHaveGainRelics = data.haveGainRelics;
            if (loadRelicLevels != null)
            {
                // SaveDeck content takes precedence
                foreach (var relicId in relicLevels.Keys.ToList())
                {
                    if (loadRelicLevels.ContainsKey(relicId))
                    {
                        relicLevels[relicId] = loadRelicLevels[relicId];
                    }

                    if (loadHaveGainRelics.ContainsKey(relicId))
                    {
                        haveGainedRelic[relicId] = loadHaveGainRelics[relicId];
                    }
                }
            }
        }

        public void SaveData(GameData data)
        {
            data.relicLevels = relicLevels;
            data.haveGainRelics = haveGainedRelic;
        }
    }
}