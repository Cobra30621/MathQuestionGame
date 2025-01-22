using System.Collections.Generic;
using Characters;
using Effect;
using Effect.Parameters;
using Effect.Power;
using UnityEngine;

namespace Power.Buff
{
    public class FrenzyPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Frenzy;

        public override void OnBeAttacked(DamageInfo info)
        {
            var source = info.EffectSource.SourceCharacter;
            var originalDamage = info.DamageValue;
            int stackAmount = Mathf.CeilToInt(originalDamage * 0.5f);
      
            if (source != null)
            {
                EffectExecutor.ExecuteImmediately(
                    new ApplyPowerEffect(stackAmount, PowerName.Strength, 
                        new List<CharacterBase>(){Owner}, GetEffectSource()));
                // 觸發後減層數 1 
                EffectExecutor.ExecuteImmediately(
                    new ApplyPowerEffect(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetEffectSource()));
            }
        }
    }
}