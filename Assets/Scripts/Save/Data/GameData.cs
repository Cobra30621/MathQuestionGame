using System.Collections.Generic;
using Data;
using NueGames.Data.Collection;
using NueGames.Encounter;
using Question;

namespace Data
{
    public class GameData
    {
        /// <summary>
        /// 玩家資訊
        /// </summary>
        public PlayerData PlayerData;
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
    }
}