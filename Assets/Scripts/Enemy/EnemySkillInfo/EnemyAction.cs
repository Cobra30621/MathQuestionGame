using System;
using System.Collections.Generic;
using Card;
using rStarTools.Scripts.StringList;

namespace Enemy.EnemySkillInfo
{
    [Serializable]
    public class EnemyAction : DataBase<EnemyActionOverview>
    {
        public string ID;
        
        public string SkillID;

        public int Target;
        public string ps;
        public int CD;

        public List<SkillInfo> skillInfos;

    }
}