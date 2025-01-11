using System;
using System.Collections.Generic;

namespace Characters.Enemy.Data
{
    [Serializable]
    public class EnemySkillData
    {
        public string SkillID;

        public string ps;
        public int CD;
        public string Intention;

        public List<string> skillIDs;
    }
}