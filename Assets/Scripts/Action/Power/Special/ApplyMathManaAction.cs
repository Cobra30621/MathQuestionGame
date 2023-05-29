using NueGames.Action;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace Action.Power
{
    public class ApplyMathManaAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.ApplyBlock;

        public ApplyMathManaAction(int value)
        {
            BaseValue = value;
        }


        public override void DoAction()
        {
            CombatManager.CurrentMainAlly.CharacterStats.ApplyPower(PowerType.MathMana, AdditionValue);
            
            UIManager.Instance.CombatCanvas.OnMathManaChange();
            // PlayFx(FxType.Block, Target.transform);
        }
    }
}