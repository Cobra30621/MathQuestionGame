using System;
using System.Collections.Generic;
using rStarTools.Scripts.StringList;
using UnityEngine.Serialization;

namespace Characters.Enemy.Data
{
    /// <summary>
    /// 敵人資料類別
    /// 繼承自 DataBase<EnemyDataOverview>，提供與概覽資料的綁定與序列化
    /// </summary>
    [Serializable]
    public class EnemyData : DataBase<EnemyDataOverview>
    {
        /// <summary>敵人唯一識別碼，對應資料來源的 ID 欄位</summary>
        public string ID;

        /// <summary>敵人名稱</summary>
        public string Lang;

        /// <summary>敵人意圖 ID，可包含多個以逗號分隔</summary>
        public string EnemyIntentionID;

        /// <summary>敵人等級，用於區分難度或關卡進度</summary>
        public string Level;

        /// <summary>預置體路徑或名稱，用於在場景中實例化敵人</summary>
        public string Prefab;

        /// <summary>開戰時觸發的意圖 ID，可包含多個以逗號分隔</summary>
        public string StartBattleIntentionID;

        /// <summary>敵人最大生命值</summary>
        public int MaxHp;

        /// <summary>是否為 Boss 類型敵人，影響行為與 UI 顯示</summary>
        public bool IsBoss;
        
        /// <summary>解析後的開戰意圖 ID 列表</summary>
        public List<string> startBattleIntentionIDs = new List<string>();

        /// <summary>解析後的敵人意圖 ID 列表</summary>
        public List<string> enemyIntentionIDs;
    }
}