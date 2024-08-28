using System;
using System.Collections.Generic;
using System.Linq;
using Data.Encounter;
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
        public List<EncounterName> GetEncounterListByWeight(int count)
        {
            List<EnemyEncounterClip> randomClipList = WeightedRandom.GetWeightedRandomObjects(weightClip, count);
            
            var encounterList = new List<EncounterName>();
            foreach (var clip in randomClipList)
            {
                encounterList.Add(clip.Encounter);
            }

            return  encounterList;

        }
    }
    
    /// <summary>
    /// 遭遇敵人清單(包含權重)
    /// </summary>
    [Serializable]
    public class EnemyEncounterClip : IWeightedObject
    {
        [SerializeField] private EncounterName encounter;
        [SerializeField] private int weight;
        /// <summary>
        /// 一場戰鬥的敵人清單
        /// </summary>
        public EncounterName Encounter => encounter;
        /// <summary>
        /// 權重(機率)
        /// </summary>
        public int Weight => weight;

        public void SetEncounter(EncounterName name)
        {
            encounter = name;
        }
    }
}