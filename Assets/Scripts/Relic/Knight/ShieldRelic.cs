using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;
using Power;
using Relic.Data;

namespace Relic.Knight
{
    public class ShieldRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Shield;

        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                if(IsMaxLevel())
                { 
                    EffectExecutor.AddEffect(new ApplyPowerEffect(
                        2, PowerName.Shield, new List<CharacterBase>() {MainAlly},
                        GetEffectSource()));
                }
                else
                {
                    EffectExecutor.AddEffect(new ApplyPowerEffect(
                        1, PowerName.Shield, new List<CharacterBase>() {MainAlly},
                        GetEffectSource()));
                }
                
            }
        }
       
    }
}