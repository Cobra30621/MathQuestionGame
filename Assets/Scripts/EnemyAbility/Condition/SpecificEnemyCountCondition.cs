using NueGames.Combat;
using Sirenix.OdinInspector;

namespace EnemyAbility
{
    public class SpecificEnemyCountCondition : ConditionBase
    {
        [InfoBox("場上敵人最多數量為 4 個")]
        public int lessOrEqualCount;

        public EnemyData EnemyData;

        public override ConditionBase GetCopy()
        {
            return new SpecificEnemyCountCondition()
            {
                EnemyData = EnemyData,
                lessOrEqualCount = lessOrEqualCount,
                enemy = enemy
            };
        }

        public override bool Judge()
        {
            return lessOrEqualCount >= CombatManager.Instance.GetEnemyCount(EnemyData);
        }
    }
}