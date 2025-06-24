using System.Collections.Generic;
using Card.Data;
using Characters.Ally;
using Encounter;
using Relic.Data;

namespace Save.Data
{
    /// <summary>
    /// 儲存單場遊戲進行中的資料，例如角色狀態、卡牌、地圖進度等。
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// 玩家選擇的角色名稱。
        /// </summary>
        public AllyName AllyName;

        /// <summary>
        /// 玩家角色的當前生命與相關狀態資料。
        /// </summary>
        public AllyHealthData AllyHealthData;

        /// <summary>
        /// 玩家目前持有的卡牌清單（以卡牌名稱表示）。
        /// </summary>
        public List<CardName> CardNames;

        /// <summary>
        /// 玩家目前持有的遺物清單（以遺物名稱表示）。
        /// </summary>
        public List<RelicName> Relics;
        
        /// <summary>
        /// 地圖的序列化資料（JSON 格式，用於還原地圖結構）。
        /// </summary>
        public string MapJson;
        
        /// <summary>
        /// 所在關卡的名稱。
        /// </summary>
        public string StageName;

        /// <summary>
        /// 玩家目前所在的地圖索引位置（用於判斷關卡進度）。
        /// </summary>
        public int CurrentMapIndex;
        
        /// <summary>
        /// 目前所待的地圖事件資訊（敵人清單）。
        /// </summary>
        public MapEncounter MapEncounter;
    }
}