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
        private int amount = 3;

        public override void OnTurnEnd(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    amount, PowerName.Block, new List<CharacterBase>() {MainAlly},
                    GetActionSource()));
            }
        }
       
    }
}