using System;
using System.Collections.Generic;
using NueGames.Encounter;
using UnityEngine;

namespace NueGames.Data.Encounter
{
    [Serializable]
    public class EnemyEncounterList
    {
        public List<EnemyEncounterClip> weightClip;
        
        /// <summary>
        /// 依照權重，取得指定數量遭遇的敵人
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<string> GetEncounterListByWeight(int count)
        {
            List<EnemyEncounterClip> randomClipList = WeightedRandom.GetWeightedRandomObjects(weightClip, count);
            
            List<EnemyEncounter> encounterList = new List<EnemyEncounter>();
            foreach (var clip in randomClipList)
            {
                encounterList.Add(clip.Encounter);
            }

            return  EncounterManager.Instance.EnemyEncounterToGuid(encounterList);

        }
    }
    
    /// <summary>
    /// 遭遇敵人清單(包含權重)
    /// </summary>
    [Serializable]
    public class EnemyEncounterClip : IWeightedObject
    {
        [SerializeField] private EnemyEncounter encounter;
        [SerializeField] private int weight;
        /// <summary>
        /// 一場戰鬥的敵人清單
        /// </summary>
        public EnemyEncounter Encounter => encounter;
        /// <summary>
        /// 權重(機率)
        /// </summary>
        public int Weight => weight;
    }
}