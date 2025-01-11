using System;

namespace Relic.Data
{
    [Serializable]
    public class RelicInfo
    {
        public RelicName relicName;
        
        public RelicData data;

        public RelicSaveInfo relicSaveInfo;


        public RelicInfo()
        {
        }

        public RelicInfo(RelicName relicName, RelicData data, RelicSaveInfo relicSaveInfo)
        {
            this.relicName = relicName;
            this.data = data;
            this.relicSaveInfo = relicSaveInfo;
        }


        public string GetDescription()
        {
            return data.GetDescription(relicSaveInfo.Level);
        }
    }
}