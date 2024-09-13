using System.Collections.Generic;
using Card;
using Card.Data;
using NueGames.Relic;
using Relic;

namespace Data
{
    public class PermanentGameData
    {
        public int money;

        public int stone;
        
        // 卡片等級字典
        public Dictionary<string, CardSaveInfo> cardLevels;
        
        // 遺物等級資料
        public Dictionary<RelicName, RelicSaveInfo> relicInfo;
    }
}