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
        
        
        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnEnd += OnTurnEnd;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnEnd -= OnTurnEnd;
        }
        
        


        protected override void OnTurnEnd(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                int fireAmount = Amount;
                // if (CombatManager.Instance.MainAlly.HasPower(PowerName.Kindle))
                // {
                //    fireAmount = Amount * 2;
                // }

                // 造成傷害
                var damageInfo = new DamageInfo(fireAmount, GetActionSource(), fixDamage: true);
                EffectExecutor.AddEffect(new DamageEffect(damageInfo, 
                    new List<CharacterBase>() {Owner}));
                
                // 燒血後減層數 1 
                EffectExecutor.AddEffect(
                    new ApplyPowerEffect(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
                
                
            }
        }
    }
}