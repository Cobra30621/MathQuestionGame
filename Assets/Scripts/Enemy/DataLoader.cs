using Card;
using Enemy.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class DataLoader : ScriptableObject
    {
        [FormerlySerializedAs("enemyInfoOverview")] public EnemyDataOverview enemyDataOverview;

        [FormerlySerializedAs("enemySkillInfoOverview")] [FormerlySerializedAs("enemyActionOverview")] public EnemySkillDataOverview enemySkillDataOverview;

        public SkillData skillData;
        
        public CardLevelData cardLevelData;
    }
}