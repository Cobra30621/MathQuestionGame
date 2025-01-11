using Sirenix.OdinInspector;

namespace Reward.Data
{
    public enum RewardType
    {
        [LabelText("金幣")]
        Money = 0,
        [LabelText("寶石")]
        Stone = 1,
        [LabelText("卡片")]
        Card = 2,
        [LabelText("遺物")]
        Relic = 3,

    }
}