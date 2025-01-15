using System.Collections.Generic;
using Characters.Ally;
using Encounter;
using Relic.Data;

namespace Save.Data
{
    public class GameData
    {
        /// <summary>
        /// 玩家選擇的角色 ID
        /// </summary>
        public string AllyDataGuid;
        
        
        public AllyHealthData AllyHealthData;
        
        /// <summary>
        /// 卡牌資料
        /// </summary>
        public List<string> CardDataGuids;
        
        /// <summary>
        /// 遺物資料
        /// </summary>
        public List<RelicName> Relics;

        /// <summary>
        /// 關卡名稱
        /// </summary>
        public string StageName;
        
        /// <summary>
        /// 地圖資訊
        /// </summary>
        public string MapJson;
        /// <summary>
        /// 地圖事件
        /// </summary>
        public MapEncounter MapEncounter;
        /// <summary>
        /// 現在在第幾個地圖
        /// </summary>
        public int CurrentMapIndex;
        
        
    }
}