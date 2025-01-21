using System.Collections.Generic;
using Characters;
using Effect;
using Effect.Parameters;
using Effect.Power;
using UnityEngine;

namespace Power.Buff
{
    public class TrapPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Trap;

        public override void OnAttacked(DamageInfo info)
        {
            var source = info.EffectSource.SourceCharacter;
            Debug.Log(source);
            if (source != null)
            {
                EffectExecutor.AddEffect(
                    new ApplyPowerEffect(2, PowerName.Weak, 
                        new List<CharacterBase>() {info.EffectSource.SourceCharacter}, GetEffectSource()));
                // 觸發後減層數 1 
                EffectExecutor.AddEffect(
                    new ApplyPowerEffect(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetEffectSource()));
            }
        }
    }
}