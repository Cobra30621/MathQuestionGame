// File: /Users/cobra/Desktop/Unity/Develop/MathQuestionGame/Assets/Scripts/Relic/RelicManager.cs

using System.Collections.Generic;
using System.Linq;
using Economy;
using Log;
using Managers;
using NueGames.Data.Containers;
using Relic.Data;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Relic
{
    /// <summary>
    /// Manages the collection and upgrade of relics in the game.
    /// </summary>
    public class RelicManager : SerializedMonoBehaviour, IDataPersistence
    {
        protected UIManager UIManager => UIManager.Instance;

        public UnityEvent<Dictionary<RelicName, RelicBase>> OnRelicUpdated =
            new UnityEvent<Dictionary<RelicName, RelicBase>>();

        public Dictionary<RelicName, RelicBase> CurrentRelicDict = new Dictionary<RelicName, RelicBase>();
        public RelicsData relicsData;

        [Required]
        public RelicLevelHandler relicLevelHandler;

     

        /// <summary>
        /// Gains a relic for the player.
        /// </summary>
        /// <param name="relicNames">The names of the relics to gain.</param>
        public void GainRelic(List<RelicName> relicNames)
        {
            foreach (var relicName in relicNames)
            {
                GainRelic(relicName);
            }
        }
        
        /// <summary>
        /// Gains a single relic for the player.
        /// </summary>
        /// <param name="relicName">The name of the relic to gain.</param>
        [Button]
        public void GainRelic(RelicName relicName)
        {
            var relicBase = RelicGenerator.GetRelic(relicName);
            var info = GetRelicInfo(relicName);
            relicBase.RelicInfo = info;
            CurrentRelicDict[relicName] = relicBase;
            
            relicLevelHandler.OnGainRelic(relicName);
            
            OnRelicUpdated.Invoke(CurrentRelicDict);
            
            EventLogger.Instance.LogEvent(LogEventType.Relic, $"獲得遺物 - {info.data.Title}");
        }

        public void RemoveAllRelic()
        {
            CurrentRelicDict.Clear();
            OnRelicUpdated.Invoke(CurrentRelicDict);
        }
        


        /// <summary>
        /// Retrieves a list of relics to display in the shop.
        /// </summary>
        /// <returns>A list of relic information for the shop.</returns>
        public List<RelicInfo> GetShopRelicInfo()
        {
            return GetAllRelicInfo().Where(r => !r.data.IsDeveloping).ToList();
        }
        
        /// <summary>
        /// Retrieves a list of all relics in the game.
        /// </summary>
        /// <returns>A list of relic information for all relics.</returns>
        public List<RelicInfo> GetAllRelicInfo()
        {
            var relicInfos = relicsData.RelicDict.Keys.
                ToList().
                ConvertAll(GetRelicInfo).
                ToList();

            return relicInfos;
        }


        public RelicInfo GetRelicInfo(RelicName targetRelic)
        {
            RelicData data = relicsData.GetRelicData(targetRelic);
       
            var info = new RelicInfo()
            {
                relicName = targetRelic,
                data = data,
                relicSaveInfo = relicLevelHandler.GetRelicSaveInfo(targetRelic)
            };

            return info;
        }
        

        /// <summary>
        /// Upgrades a relic for the player.
        /// </summary>
        /// <param name="relicName">The name of the relic to upgrade.</param>
        [Button]
        public void UpgradeRelic(RelicName relicName)
        {
            var level = relicLevelHandler.UpgradeRelic(relicName);
            if(CurrentRelicDict.TryGetValue(relicName, out var value))
                value.RelicInfo.relicSaveInfo.Level = level;

            OnRelicUpdated.Invoke(CurrentRelicDict);
        }
        
        public Dictionary<CoinType, int> UpgradeNeedCost()
        {
            return relicsData.UpgradeCost;
        }
        
        public void LoadData(GameData data)
        {
            RemoveAllRelic();
            
            GainRelic(data.Relics);
        }

        public void SaveData(GameData data)
        {
            data.Relics = CurrentRelicDict.Keys.ToList();
        }
    }
}