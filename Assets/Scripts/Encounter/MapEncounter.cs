using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NueGames.Data.Encounter;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Encounter
{
    /// <summary>
    /// 地圖遭遇事件資料
    /// </summary>
    [Serializable]
    public class MapEncounter
    {
        public List<string> enemyList;
        public List<string> eliteEnemyList;
        public List<string> bossList;

        public int addCount = 10;
        
        public void GeneratorStageData(EncounterStage stage)
        {
            enemyList = new List<string>();
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
            string encounterName = enemyList[0];
            enemyList.Remove(encounterName);
            
            EnemyEncounter encounter = EncounterManager.Instance.GetEnemyEncounter(encounterName);
            return encounter;
        }

        public EnemyEncounter GetEliteEncounter()
        {
            string encounterName = eliteEnemyList[0];
            eliteEnemyList.Remove(encounterName);
            
            EnemyEncounter encounter = EncounterManager.Instance.GetEnemyEncounter(encounterName);
            return encounter;
        }
        
        public EnemyEncounter GetBossEncounter()
        {
            string encounterName = bossList[0];
            bossList.Remove(encounterName);
            
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