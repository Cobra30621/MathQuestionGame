using System;
using rStarTools.Scripts.StringList;

namespace Stage
{
    [Serializable]
    public class StageName : NameBase<StageDataOverview>
    {
        protected override string LabelText => "關卡名稱";
    }
}