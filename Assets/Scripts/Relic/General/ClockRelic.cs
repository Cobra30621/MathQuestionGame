using Combat;
using Relic.Data;
using UnityEngine;

namespace Relic.General
{
    public class ClockRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Clock;
        private int passed_turn = 0;
        private int mana = 1;

        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                passed_turn++;
            }

            if (passed_turn == 3)
            {
                mana = IsMaxLevel()? 2 : 1;
                CombatManager.AddMana(mana);
                passed_turn = 0;
            }
        }
       
    }
}