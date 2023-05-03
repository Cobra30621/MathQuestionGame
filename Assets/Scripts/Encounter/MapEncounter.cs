using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NueGames.Data.Encounter;
using UnityEngine;

namespace NueGames.Encounter
{
    /// <summary>
    /// 地圖遭遇事件資料
    /// </summary>
    [Serializable]
    public class MapEncounter
    {
        public List<EnemyEncounterName> enemyList;
        public List<EnemyEncounterName> eliteEnemyList;
        public List<EnemyEncounterName> bossList;
        
        public int addCount = 10;
        
        public void GeneratorStageData(EncounterStage stage)
        {
            enemyList = new List<EnemyEncounterName>();
            Debug.Log($"stage {stage}");
            int weakEnemyCount = stage.weakEnemyCount;
            enemyList.AddRange(stage.weakEnemies.GetEncounterListByWeight(weakEnemyCount));
            enemyList.AddRange(stage.strongEnemies.GetEncounterListByWeight(addCount));

            eliteEnemyList = stage.eliteEnemies.GetEncounterListByWeight(addCount);

            bossList = stage.bossEnemies.GetEncounterListByWeight(1);
        }
        
        /// <summary>
        /// 取得一般敵人遭遇
        /// </summary>
        /// <returns></returns>
        public EnemyEncounter GetEnemyEncounter()
        {
            EnemyEncounterName encounterName = enemyList[0];
            enemyList.Remove(encounterName);
            
            EnemyEncounter encounter = EncounterManager.Instance.GetEnemyEncounter(encounterName);
            
            return encounter;
        }
        
        
        public string ToJson()
        {
            Debug.Log("enemyList" +  JsonConvert.SerializeObject(enemyList));
            return JsonConvert.SerializeObject(this);
        }
    }
}