using System;
using NueGames.Data.Containers;
using NueGames.Relic;

namespace Relic
{
    [Serializable]
    public class RelicInfo
    {
        public RelicName relicName;
        
        public RelicData data;

        public RelicLevelInfo relicLevelInfo;
        
        public string GetDescription()
        {
            return data.GetDescription(relicLevelInfo.Level);
        }
    }
}