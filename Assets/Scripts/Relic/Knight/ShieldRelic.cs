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
                EffectExecutor.AddAction(new ApplyPowerEffect(
                    amount, PowerName.Block, new List<CharacterBase>() {MainAlly},
                    GetActionSource()));
            }
        }
       
    }
}