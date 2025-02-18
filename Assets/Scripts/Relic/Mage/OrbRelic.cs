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
            return rawValue + 1;
        }
    }
}