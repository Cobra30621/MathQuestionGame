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
            
            UIManager.RelicCanvas.OnGainRelic(CurrentRelicDict);
            
            Debug.Log("GainRelic: " + targetRelic);
        }

        /// <summary>
        /// 取得所有的遺物清單
        /// </summary>
        /// <returns></returns>
        public List<RelicInfo> GetAllRelicInfo()
        {
            var datas = relicsData.RelicList.Where(x => !x.IsDeveloping).ToList();

            var relicInfos = datas.ConvertAll(x => GetRelicInfo(x.RelicName)).ToList();

            return relicInfos;
        }
        

        private RelicInfo GetRelicInfo(RelicName targetRelic)
        {
            RelicData data = relicsData.RelicList.FirstOrDefault(x => x.RelicName == targetRelic);
            int level = relicLevelHandler.GetRelicLevel(targetRelic);
            bool hasGained = relicLevelHandler.HasGainRelic(targetRelic);

            var info = new RelicInfo()
            {
                relicName = targetRelic,
                data = data,
                level = level,
                haveGain = hasGained
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
            CurrentRelicDict[relicName].RelicInfo.level = level;

            UIManager.RelicCanvas.OnGainRelic(CurrentRelicDict);
        }
        
        public List<RelicName> GetRelicNames()
        {
            return CurrentRelicDict.Keys.ToList();
        }
        
    }
}