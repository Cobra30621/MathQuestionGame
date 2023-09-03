using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Data.Containers;
using NueGames.Managers;
using NueGames.Relic;
using UnityEngine;

namespace Managers
{
    public class RelicManager : Singleton<RelicManager>
    {
        protected UIManager UIManager => UIManager.Instance;
        public List<RelicClip> CurrentRelicList = new List<RelicClip>();
        public RelicsData relicsData;
        
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
        public void GainRelic(RelicName targetRelic)
        {
            RelicBase relicBase = RelicGenerator.GetRelic(targetRelic);
            RelicData data = relicsData.RelicList.FirstOrDefault(x => x.RelicName == targetRelic);
            RelicClip relicClip = new RelicClip(relicBase, data);
            
            CurrentRelicList.Add(relicClip);
            UIManager.RelicCanvas.OnGainRelic(relicClip);
        }

        public List<RelicName> GetRelicNames()
        {
            var relicNames = new List<RelicName>();

            foreach (var relicClip in CurrentRelicList)
            {
                relicNames.Add(relicClip.Data.RelicName);
            }

            return relicNames;
        }
        
        #region Debug 用
        /// <summary>
        /// 印出所有的遺物
        /// </summary>
        public void PrintCurrentRelicList()
        {
            foreach (var relicClip in CurrentRelicList)
            {
                Debug.Log(relicClip);
            }
        }

        #endregion
    }
}