using Action.Power;
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
            GameActionExecutor.AddToBottom(new ApplyBlockAction(
                Value,  TargetList, GetActionSource()));
        }
    }
}