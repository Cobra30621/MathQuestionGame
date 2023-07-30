using NueGames.Action;
using NueGames.Managers;
using NueGames.Power;

namespace CardAction
{
    public class Block : CardActionBase
    {
        public int AddValue;
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                AddValue, PowerName.Block, TargetList, GetActionSource()));
        }
    }

}