using Sirenix.OdinInspector;

namespace Reward
{
    /// <summary>
    /// 道具獲取方式
    /// </summary>
    public enum ItemGainType
    {
        [LabelText("從使用中角色")]
        Character, // 角色
        [LabelText("從共用清單")]
        Common, // 通用
        [LabelText("指定")]
        Specify // 指定
    }
}