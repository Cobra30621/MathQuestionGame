using System.Collections.Generic;
using Card;
using NueGames.Relic;

namespace Data
{
    public class PermanentGameData
    {
        public int money;

        public int stone;
        
        // 卡片等級字典
        public Dictionary<string, CardSaveLevel> cardLevels;
        
        // 遺物等級資料
        public Dictionary<RelicName, RelicLevelInfo> relicInfo;
    }
}