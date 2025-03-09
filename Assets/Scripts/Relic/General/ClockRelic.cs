using Combat;
using Relic.Data;
using UnityEngine;

namespace Relic.General
{
    public class ClockRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Clock;
        private int passed_turn = 0;

        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                passed_turn++;
            }

            if (passed_turn == 4)
            {
                CombatManager.AddMana(1);
                passed_turn = 0;
            }
        }
       
    }
}