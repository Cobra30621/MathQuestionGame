using System.Collections.Generic;
using System.Linq;
using Card;
using Enemy.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
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
        

        public List<Data.EnemySkillData> GetEnemySkillInfos(List<string> ids)
        {
            return ids.ConvertAll(GetEnemySkillInfo).ToList();
        } 
        

        public Data.EnemySkillData GetEnemySkillInfo(string id)
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