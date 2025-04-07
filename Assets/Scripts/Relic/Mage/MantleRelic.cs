using Effect;
using Relic.Data;
using Effect.Power;
using Power;
using Combat;
namespace Relic.Mage
{
    public class MantleRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Mantle;
        public override void OnTurnStart(TurnInfo info)
        {
            var targets = CombatManager.EnemiesForTarget();
            if (IsMaxLevel())
            {
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    3, PowerName.Weak, targets,
                    GetEffectSource()));
            }
            else
            {
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    1, PowerName.Weak, targets,
                    GetEffectSource()));
            }
        }
    }
}