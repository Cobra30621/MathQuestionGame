using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Damage;
using Effect.Parameters;
using Effect.Power;

namespace Power.Buff
{


    /// <summary>
    /// 燃燒能力
    /// </summary>
    public class FirePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Fire;
   

        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                int poisonAmount = Amount;

                // 造成傷害
                var damageInfo = new DamageInfo(poisonAmount, GetEffectSource(), fixDamage: true);
                EffectExecutor.ExecuteImmediately(new DamageEffect(damageInfo, 
                    new List<CharacterBase>() {Owner}));
                
                // 扣血後減層數 1 
                EffectExecutor.ExecuteImmediately(
                    new ApplyPowerEffect(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetEffectSource()));
            }
        }
    }
}