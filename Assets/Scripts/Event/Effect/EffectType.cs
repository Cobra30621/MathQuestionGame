using Sirenix.OdinInspector;

namespace NueGames.Event.Effect
{
    public enum EffectType
    {
        [LabelText("離開")]
        Leave = 0,
        [LabelText("獎勵")]
        Reward = 1,
        [LabelText("數學答題")]
        MathQuestion = 3,
        [LabelText("改變血量")]
        ChangeHealth,
        
    }
}