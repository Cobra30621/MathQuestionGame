using System.Collections.Generic;
using Card.Data;
using Question.Data;
using Relic;
using Relic.Data;

namespace Save.Data
{
    public class PermanentGameData
    {
        public int money;

        public int stone;
        
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