using System;
using Sirenix.OdinInspector;

namespace NueGames.Enums
{
    /// <summary>
    /// 牌堆類型
    /// </summary>
    [Serializable]
    public enum PileType
    {
        [LabelText("抽牌堆")]
        Draw = 0,
        [LabelText("手牌")]
        Hand = 1,
        [LabelText("棄牌堆")]
        Discard = 2,
        [LabelText("消耗堆")]
        Exhaust = 3
    }
}