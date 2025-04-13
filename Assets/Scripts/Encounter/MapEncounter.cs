using System;
using System.Collections.Generic;
using Encounter.Data;
using Log;
using Map;
using UnityEngine;

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
        public EncounterName GetEnemyEncounter()
        {
            if (enemyList == null || enemyList.Count == 0)
            {
                enemyList = new List<EncounterName>();
                var stage = EncounterManager.Instance.EncounterStage;
                enemyList.AddRange(stage.strongEnemies.GetEncounterListByWeight(addCount));
            }

            var encounterName = enemyList[0];
            
            return encounterName;
        }

        public EncounterName GetEliteEncounter()
        {
            if (eliteEnemyList == null || eliteEnemyList.Count == 0)
            {
                var stage = EncounterManager.Instance.EncounterStage;
                eliteEnemyList = stage.eliteEnemies.GetEncounterListByWeight(addCount);
            }

            var encounterName = eliteEnemyList[0];
            return encounterName;
        }

        public EncounterName GetBossEncounter()
        {
            if (bossList == null || bossList.Count == 0)
            {
                var stage = EncounterManager.Instance.EncounterStage;
                bossList = stage.bossEnemies.GetEncounterListByWeight(1);
            }

            var encounterName = bossList[0];
            return encounterName;
        }

        /// <summary>
        /// 完成此關卡
        /// </summary>
        /// <param name="nodeType"></param>
        public void CompleteRoom(NodeType nodeType, EncounterName encounterName)
        {
            switch (nodeType)
            {
                case NodeType.MinorEnemy:
                    enemyList.RemoveAt(0);
                    break;
                case NodeType.EliteEnemy:
                    eliteEnemyList.RemoveAt(0);
                    break;
                case NodeType.Boss:
                    bossList.RemoveAt(0);
                    break;
                default:
                    Debug.LogError($"Wrong Node Type {nodeType} for EnemyEncounter");
                    break;
            }
            
            EventLogger.Instance.LogEvent(LogEventType.MapEncounter, 
                $"擊敗遭遇: {encounterName}({nodeType})");
        }
    }
}
