using Sirenix.OdinInspector;

namespace MapEvent.Effect
{
    public enum EventEffectType
    {
        [LabelText("離開")]
        Leave = 0,
        [LabelText("獎勵")]
        Reward = 1,
        [LabelText("數學答題")]
        MathQuestion = 3,
        [LabelText("改變血量")]
        ChangeHealth = 4,
        [LabelText("支付後獲得獎勵")]
        PayAndGain = 5,
        
    }
}