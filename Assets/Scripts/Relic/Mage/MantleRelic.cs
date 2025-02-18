using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;
using Power;
using Relic.Data;

namespace Relic.Mage
{
    public class MantleRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Mantle;
        private bool isFirstAttacked = true;
        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                isFirstAttacked = true;
            }
        }
        
        public override float AtDamageReceive(float damage)
        {
            if (isFirstAttacked)
            {
                isFirstAttacked = false;
                return damage * 0.5f;
            }
            return damage;
        }
    }
}