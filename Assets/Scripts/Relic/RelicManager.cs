using System.Collections.Generic;
using System.Linq;
using NueGames.Data.Containers;
using NueGames.Managers;
using NueGames.Relic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Relic
{
    public class RelicManager : SerializedMonoBehaviour
    {
        protected UIManager UIManager => UIManager.Instance;

        public UnityEvent<Dictionary<RelicName, RelicBase>> OnRelicUpdated =
            new UnityEvent<Dictionary<RelicName, RelicBase>>();

        public Dictionary<RelicName, RelicBase> CurrentRelicDict = new Dictionary<RelicName, RelicBase>();
        public RelicsData relicsData;

        [Required]
        public RelicLevelHandler relicLevelHandler;

     

        /// <summary>
        /// 玩家獲得遺物
        /// </summary>
        /// <param name="targetRelic"></param>
        /// <returns></returns>
        public void GainRelic(List<RelicName> relicNames)
        {
            foreach (var relicName in relicNames)
            {
                GainRelic(relicName);
            }
        }
        
        /// <summary>
        /// 玩家獲得遺物
        /// </summary>
        /// <param name="targetRelic"></param>
        /// <returns></returns>
        [Button]
        public void GainRelic(RelicName targetRelic)
        {
            var relicBase = RelicGenerator.GetRelic(targetRelic);
            var info = GetRelicInfo(targetRelic);
            relicBase.RelicInfo = info;
            CurrentRelicDict[targetRelic] = relicBase;
            
            relicLevelHandler.OnGainRelic(targetRelic);
            
            OnRelicUpdated.Invoke(CurrentRelicDict);
        }

        /// <summary>
        /// 取得所有的遺物清單
        /// </summary>
        /// <returns></returns>
        public List<RelicInfo> GetAllRelicInfo()
        {
            var relicInfos = relicsData.RelicDict.Keys.
                ToList().
                ConvertAll(GetRelicInfo).
                ToList();

            return relicInfos;
        }

        public void SetRelicsInfo(List<RelicInfo> infos)
        {
            foreach (var info in infos)
            {
                relicLevelHandler.relicLevelInfos[info.relicName] = info.relicLevelInfo;

                if (CurrentRelicDict.TryGetValue(info.relicName, out var value))
                {
                    value.RelicInfo = info;
                }
            }
            
        }
        

        private RelicInfo GetRelicInfo(RelicName targetRelic)
        {
            RelicData data = relicsData.GetRelicData(targetRelic);
       
            var info = new RelicInfo()
            {
                relicName = targetRelic,
                data = data,
                relicLevelInfo = relicLevelHandler.GetRelicSaveInfo(targetRelic)
            };

            return info;
        }
        

        /// <summary>
        /// 升級卡牌
        /// </summary>
        /// <param name="relicName"></param>
        [Button]
        public void UpgradeRelic(RelicName relicName)
        {
            var level = relicLevelHandler.UpgradeRelic(relicName);
            CurrentRelicDict[relicName].RelicInfo.relicLevelInfo.Level = level;

            OnRelicUpdated.Invoke(CurrentRelicDict);
        }
        
        public List<RelicName> GetRelicNames()
        {
            return CurrentRelicDict.Keys.ToList();
        }
        
    }
}