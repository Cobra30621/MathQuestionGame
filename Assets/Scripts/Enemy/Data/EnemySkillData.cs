using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Enemy.Data
{
    [Serializable]
    public class EnemySkillData
    {
        public string ID;
        
        public string SkillID;

        public string ps;
        public int CD;
        public string Intention;

        public List<string> skillIDs;
    }
}