using System;
using System.Collections.Generic;
using rStarTools.Scripts.StringList;

namespace Characters.Enemy.Data
{
    [Serializable]
    public class EnemyData : DataBase<EnemyDataOverview>
    {
        public string ID;
        public string Lang;
        public string EnemySkillID;
        public string Level;
        public string Prefab;
        public string StartBattleSkillID;
        public int MaxHp;
        public bool IsBoss;
        
        public List<string> startBattleSkillIDs = new List<string>();
        public List<string> enemySkillIDs;
    }
    
}