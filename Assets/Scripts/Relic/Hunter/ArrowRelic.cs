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