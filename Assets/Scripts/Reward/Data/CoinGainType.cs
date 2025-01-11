using Sirenix.OdinInspector;

namespace Reward.Data
{
    /// <summary>
    /// 貨幣獲取方式
    /// </summary>
    public enum CoinGainType
    {
        [LabelText("使用基礎掉落率")]
        NodeType, 
        [LabelText("指定數值")]
        Specify 
    }
}