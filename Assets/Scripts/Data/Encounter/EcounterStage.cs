
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Encounter.EncounterList;
using Newtonsoft.Json;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.Encounter;
using NueGames.Enums;
using NueGames.NueExtentions;
using UnityEngine;


namespace NueGames.Data.Encounter
{
    /// <summary>
    /// 一層地圖的遭遇資料
    /// </summary>
    [Serializable]
    public class EncounterStage
    {
        [Header("【敵人遭遇清單】")]
        [Tooltip("較弱的一般敵人出現數量")] 
        public int weakEnemyCount;
        [Tooltip("1.較弱的一般敵人(出現在地圖前半)")]
        public EnemyEncounterList weakEnemies;
        [Tooltip("2.較強的一般敵人(出現在地圖後半)")] 
        public EnemyEncounterList strongEnemies;
        [Tooltip("3.菁英敵人")] 
        public EnemyEncounterList eliteEnemies;
        [Tooltip("4.王")] 
        public EnemyEncounterList bossEnemies;

        // public RewardEncounterList rewards;

        
    }


    
}