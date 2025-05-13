using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;
using Power;
using Relic.Data;

namespace Relic.Hunter
{
    public class ArrowRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Arrow;
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
                        3, PowerName.Strength, new List<CharacterBase>() {MainAlly},
                        GetEffectSource()));
                }
            }
        }
    }
}