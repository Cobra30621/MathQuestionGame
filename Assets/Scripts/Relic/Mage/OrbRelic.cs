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
        public override int AtGainTurnStartMana(int rawValue)
        {
            // 是否有升級過
            if (IsMaxLevel())
            {
                return rawValue + 2; 
            }
            else
            {
                return rawValue + 1;
            }
        }
    }
}