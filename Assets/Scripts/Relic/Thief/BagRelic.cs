using Relic.Data;
using UnityEngine;

namespace Relic.Thief
{
    public class BagRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Bag;
        public override int AtGainTurnStartDraw(int value)
        {
            Debug.Log("BagRelic AtGainTurnStartDraw");
            Debug.Log(value);
            return value + 1;
        }
    }
}