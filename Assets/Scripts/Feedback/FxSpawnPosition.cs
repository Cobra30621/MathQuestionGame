using System;
using Sirenix.OdinInspector;

namespace Feedback
{
    [Serializable]
    public enum FxSpawnPosition
    {
        [LabelText("每一個作用目標")]
        EachTarget = 0,
        [LabelText("玩家身上")]
        Ally = 1, 
        [LabelText("畫面中間")]
        ScreenMiddle = 2,
        [LabelText("敵人中間")]
        EnemyMiddle = 3, 
    }
}