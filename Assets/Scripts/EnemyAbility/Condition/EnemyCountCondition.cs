using NueGames.Combat;
using Sirenix.OdinInspector;

namespace EnemyAbility
{
    public class EnemyCountCondition : ConditionBase
    {
        [InfoBox("場上敵人最多數量為 4 個")]
        public int lessOrEqualCount;

        public override ConditionBase GetCopy()
        {
            return new EnemyCountCondition()
            {
                lessOrEqualCount = lessOrEqualCount,
                enemy = enemy
            };
        }

        public override bool Judge()
        {
            return lessOrEqualCount >= CombatManager.Instance.Enemies.Count;
        }
    }
}