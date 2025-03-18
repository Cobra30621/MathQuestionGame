using System.Collections.Generic;
using Characters;
using Effect;
using Effect.Parameters;
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

        public override void OnAttack(DamageInfo info, List<CharacterBase> targets)
        {
            var sourceCharacter = info.EffectSource.SourceCharacter;
            
            // 如果是玩家，- 1 層
            if (sourceCharacter != null && sourceCharacter.IsCharacterType(CharacterType.Ally))
            {
                EffectExecutor.ExecuteImmediately(
                    new ApplyPowerEffect(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetEffectSource()));
            }
        }


        public override float AtDamageGive(float damage)
        {
            return damage * 2;
        }
    }
}