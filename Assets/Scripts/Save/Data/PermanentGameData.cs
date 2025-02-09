using System.Collections.Generic;
using Card.Data;
using Question.Data;
using Relic;
using Relic.Data;
using VersionControl;

namespace Save.Data
{
    public class PermanentGameData
    {
        /// <summary>
        /// 現在的金錢
        /// </summary>
        public int money;

        /// <summary>
        /// 現在的寶石
        /// </summary>
        public int stone;

        /// <summary>
        /// 遊戲存檔的版本
        /// </summary>
        public GameVersion saveVersion = new GameVersion();
        
        // 卡片等級字典
        public Dictionary<string, CardSaveInfo> cardLevels;
        
        // 遺物等級資料
        public Dictionary<RelicName, RelicSaveInfo> relicInfo;
        
        /// <summary>
        /// 問題設定
        /// </summary>
        public QuestionSetting QuestionSetting;
    }
}