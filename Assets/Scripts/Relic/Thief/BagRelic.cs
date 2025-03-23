using Relic.Data;
using UnityEngine;

namespace Relic.Thief
{
    public class BagRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Bag;
        public override int AtGainTurnStartDraw(int value)
        {
            if (IsMaxLevel())
            {
                return value + 3;
            }
            else
            {
                return value + 1;
            }
        }
    }
}