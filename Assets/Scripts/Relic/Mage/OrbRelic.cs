using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;
using Power;
using Relic.Data;

namespace Relic.Mage
{
    public class OrbRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Orb;
        public override int GainMaxMana(int rawValue)
        {
            return rawValue + 1;
        }
        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                if(IsMaxLevel())
                { 
                    EffectExecutor.AddEffect(new ApplyPowerEffect(
                        1, PowerName.Shield, new List<CharacterBase>() {MainAlly},
                        GetEffectSource()));
                }
            }
        }
    }
}