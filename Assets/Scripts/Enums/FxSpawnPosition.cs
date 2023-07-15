using System;

namespace NueGames.Enums
{
    [Serializable]
    public enum FxSpawnPosition
    {
        EachTarget = 0, // 每一個目標
        Ally = 1, // 玩家
        ScreenMiddle = 2, // 畫面中間
        EnemyMiddle = 3, // 敵人中間
    }
}