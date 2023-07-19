using NueGames.Action;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace Action.Power
{
    public class ApplyMathManaAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.ApplyBlock;

        public ApplyMathManaAction(int value)
        {
            ActionData.BaseValue = value;
        }


        protected override void DoMainAction()
        {
            CombatManager.CurrentMainAlly.ApplyPower(PowerName.MathMana, AdditionValue);

            if (AdditionValue != 0)
            {
                UIManager.Instance.CombatCanvas.OnMathManaChange();
            }
        }
    }
}