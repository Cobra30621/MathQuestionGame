using System;
using Encounter.Data.EncounterList;
using Sirenix.OdinInspector;

namespace Encounter.Data
{
    /// <summary>
    /// 一層地圖的遭遇資料
    /// </summary>
    [Serializable]
    public class EncounterStage
    {
        [LabelText("較弱的一般敵人出現數量")] 
        public int weakEnemyCount;
        [LabelText("1.較弱的一般敵人(出現在地圖前半)")]
        public EnemyEncounterList weakEnemies;
        [LabelText("2.較強的一般敵人(出現在地圖後半)")] 
        public EnemyEncounterList strongEnemies;
        [LabelText("3.菁英敵人")] 
        public EnemyEncounterList eliteEnemies;
        [LabelText("4.王")] 
        public EnemyEncounterList bossEnemies;
        
    }


    
}