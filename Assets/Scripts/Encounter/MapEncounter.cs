using System;
using System.Collections.Generic;
using Encounter.Data;

namespace Encounter
{
    /// <summary>
    /// 地圖遭遇事件資料
    /// </summary>
    [Serializable]
    public class MapEncounter
    {
        public List<EncounterName> enemyList;
        public List<EncounterName> eliteEnemyList;
        public List<EncounterName> bossList;

        public int addCount = 10;
        
        public void GeneratorStageData(EncounterStage stage)
        {
            enemyList = new List<EncounterName>();
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
        public EncounterName GetEnemyEncounter()
        {
            var encounterName = enemyList[0];
            enemyList.Remove(encounterName);
            
            return encounterName;
        }

        public EncounterName GetEliteEncounter()
        {
            var encounterName = eliteEnemyList[0];
            eliteEnemyList.Remove(encounterName);
            
            return encounterName;
        }
        
        public EncounterName GetBossEncounter()
        {
            var encounterName = bossList[0];
            bossList.Remove(encounterName);
            
            return encounterName;
        }


    }
}