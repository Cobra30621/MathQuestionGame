using System.Collections.Generic;
using Characters;
using Effect;
using Effect.Power;
using GameListener;

namespace Power.Buff
{
    /// <summary>
    /// 續力
    /// </summary>
    public class ChargePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Charge;
        
        public ChargePower()
        {
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }
        
        
        public override float AtDamageGive(float damage)
        {
            EffectExecutor.AddEffect(
                new ApplyPowerEffect(-1, PowerName, 
                    new List<CharacterBase>(){Owner}, GetEffectSource()));
            return damage*2;
        }
    }
}