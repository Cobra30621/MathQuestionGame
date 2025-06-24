using System.Collections.Generic;
using Card.Data;
using Question.Data;
using Relic;
using Relic.Data;
using VersionControl;

namespace Save.Data
{
    /// <summary>
    /// 儲存跨場次的永久遊戲資料（例如金錢、道具等），用於玩家進度保存。
    /// </summary>
    public class PermanentGameData
    {
        /// <summary>
        /// 玩家當前擁有的金錢數量。
        /// </summary>
        public int money;

        /// <summary>
        /// 玩家當前擁有的寶石數量。
        /// </summary>
        public int stone;

        /// <summary>
        /// 本次遊戲存檔的版本資訊，用於版本控制與資料相容性檢查。
        /// </summary>
        public GameVersion saveVersion = new GameVersion();
        
        /// <summary>
        /// 玩家各卡片的等級資料，使用卡片名稱作為索引鍵。
        /// </summary>
        public Dictionary<string, CardSaveInfo> cardLevels;
        
        /// <summary>
        /// 玩家各遺物的等級與升級狀態，依遺物名稱記錄。
        /// </summary>
        public Dictionary<RelicName, RelicSaveInfo> relicInfo;
        
        /// <summary>
        /// 玩家目前的題目設定，用於影響問答內容或難度。
        /// </summary>
        public QuestionSetting QuestionSetting;
    }
}