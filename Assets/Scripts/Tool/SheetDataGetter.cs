using System.Collections.Generic;
using System.Linq;
using Card;
using Enemy;
using Enemy.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tool
{
    public class SheetDataGetter : ScriptableObject
    {
        [Required]
        public EnemyDataOverview enemyDataOverview;
        [Required]
        public EnemySkillDataOverview enemySkillDataOverview;
        [Required]
        public SkillData skillData;
        [Required]
        public IntentionData intentionData;

        public Intention GetIntention(string id)
        {
            return intentionData.GetIntention(id);
        }
        

        public EnemySkillData GetEnemySkillInfo(string id)
        {
            return enemySkillDataOverview.GetEnemyAction(id);
        }

        
        public SkillInfo GetSkillInfo(string id)
        {
            return skillData.GetSkillInfo(id);
        }
    }
}