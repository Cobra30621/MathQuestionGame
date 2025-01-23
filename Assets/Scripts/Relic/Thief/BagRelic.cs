using Relic.Data;
namespace Relic.Thief
{
    public class BagRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Bag;
        // 怎麼call relic base裡的collection manager
        // CollectionManager.AddMaxHandCard(1);
    }
}