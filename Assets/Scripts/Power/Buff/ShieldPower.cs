using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;

namespace Power.Buff
{


    public class ShieldPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Shield;

        public ShieldPower()
        {

        }

        public override void OnTurnEnd(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    3 * Amount, PowerName.Block, new List<CharacterBase>() {Owner},
                    GetEffectSource()));
            }
        }
    }
}