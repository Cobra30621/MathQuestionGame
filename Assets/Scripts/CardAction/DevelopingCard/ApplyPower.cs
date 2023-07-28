using NueGames.Action;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace CardAction
{
    public class ApplyPower : CardActionBase
    {
        public int value;
        public PowerName powerName;
        protected override void DoMainAction()
        {
            var applyPowerAction = new ApplyPowerAction();
            applyPowerAction.SetValue(value, powerName, TargetList, GetActionSource());
            GameActionExecutor.Instance.AddToBottom(applyPowerAction);
        }
    }
}