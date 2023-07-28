using NueGames.Action;
using NueGames.Managers;
using NueGames.Power;

namespace CardAction
{
    public class Block : CardActionBase
    {
        public int Value;
        protected override void DoMainAction()
        {
            var applyPowerAction = new ApplyPowerAction();
            applyPowerAction.SetValue(Value, PowerName.Block, TargetList, GetActionSource());
            GameActionExecutor.Instance.AddToBottom(applyPowerAction);
        }
    }
}