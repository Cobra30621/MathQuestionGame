using System;
using System.Collections.Generic;
using rStarTools.Scripts.StringList;
using Tool;
using UnityEngine.Serialization;

namespace Enemy.Data
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
        
        public List<string> enemySkillIDs;
    }
    
}