using System.Collections.Generic;
using Action;
using Action.Power;
using Characters;
using Combat;

namespace Power.Buff
{


    public class ShieldPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Shield;

        public ShieldPower()
        {

        }

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
                
                GameActionExecutor.AddAction(new ApplyPowerAction(
                    3 * Amount, PowerName.Block, new List<CharacterBase>() {Owner},
                    GetActionSource()));
            }
        }
    }
}