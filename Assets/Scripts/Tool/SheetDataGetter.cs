using System.Collections.Generic;
using System.Linq;
using Card;
using Enemy;
using Enemy.Data;
using UnityEngine;

namespace Tool
{
    public class SheetDataGetter : ScriptableObject
    {
        
        public EnemyDataOverview enemyDataOverview;

        public EnemySkillDataOverview enemySkillDataOverview;

        public SkillData skillData;

        public IntentionData intentionData;

        public Intention GetIntention(string id)
        {
            return intentionData.GetIntention(id);
        }
        

        public List<EnemySkillData> GetEnemySkillInfos(List<string> ids)
        {
            return ids.ConvertAll(GetEnemySkillInfo).ToList();
        } 
        

        public EnemySkillData GetEnemySkillInfo(string id)
        {
            return enemySkillDataOverview.GetEnemyAction(id);
        }


        public List<SkillInfo> GetSkillInfos(List<string> ids)
        {
            return ids.ConvertAll(GetSkillInfo).ToList();
        }
        
        public SkillInfo GetSkillInfo(string id)
        {
            return skillData.GetSkillInfo(id);
        }
    }
}