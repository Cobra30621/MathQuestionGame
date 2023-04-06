using System;

namespace NueGames.Enums
{
    /// <summary>
    /// 牌堆類型
    /// </summary>
    [Serializable]
    public enum PileType
    {
        Draw,
        Hand,
        Discard,
        Exhaust
    }
}