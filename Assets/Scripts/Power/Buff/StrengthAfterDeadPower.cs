using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Parameters;
using Effect.Power;

namespace Power.Buff
{
    /// <summary>
    /// 強化
    /// </summary>
    public class StrengthAfterDeadPower : PowerBase
    {
        public override PowerName PowerName => PowerName.StrengthAfterDead;

        public StrengthAfterDeadPower()
        {
            
        }


        public override void OnDead(DamageInfo info)
        {
            List<CharacterBase> targets = CombatManager.Instance.EnemiesForTarget();

            // 對全體敵方單位施加強化
            EffectExecutor.AddEffect(
                new ApplyPowerEffect(1, PowerName.Strength, 
                    targets, GetEffectSource()));
        }
    }
}