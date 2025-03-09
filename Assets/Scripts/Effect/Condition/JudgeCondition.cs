using Sirenix.OdinInspector;

namespace Effect.Card
{
    public enum JudgeCondition
    {
        [LabelText("本回合打出的第一張卡牌")]
        FirstCardInThisTurn = 1,
        [LabelText("本回合沒給敵人造成過傷害")]
        NotHurtEnemyInThisTurn = 2,
    }
}