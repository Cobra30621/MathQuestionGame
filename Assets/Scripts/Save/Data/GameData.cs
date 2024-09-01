using System.Collections.Generic;
using Card;
using Data;
using NueGames.Data.Collection;
using NueGames.Encounter;
using NueGames.Relic;
using Question;

namespace Data
{
    public class GameData
    {
        /// <summary>
        /// 遊戲初始設定資料的 id
        /// </summary>
        public string GamePlayDataId;
        /// <summary>
        /// 玩家資訊
        /// </summary>
        public PlayerData PlayerData;
        /// <summary>
        /// 玩家金錢
        /// </summary>
        public int Money;
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
        /// <summary>
        /// 問題設定
        /// </summary>
        public QuestionSetting QuestionSetting;
        // 卡片等級字典
        public Dictionary<string, CardSaveLevel> cardLevels;
        
        
        // 遺物等級資料
        public Dictionary<RelicName, RelicLevelInfo> relicInfo;
    }
}