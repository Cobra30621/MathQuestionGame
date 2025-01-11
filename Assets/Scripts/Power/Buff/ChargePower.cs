using System.Collections.Generic;
using Characters;
using Effect;
using Effect.Power;
using GameListener;

namespace Power.Buff
{
    /// <summary>
    /// 力量
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
            EffectExecutor.AddAction(
                new ApplyPowerEffect(-1, PowerName, 
                    new List<CharacterBase>(){Owner}, GetActionSource()));
            return damage*2;
        }
    }
}