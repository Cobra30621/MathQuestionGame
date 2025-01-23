using Relic.Data;
namespace Relic.Thief
{
    public class BagRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Bag;
        public override int AtGainTurnStartDraw(int value)
        {
            return value + 1;
        }
    }
}