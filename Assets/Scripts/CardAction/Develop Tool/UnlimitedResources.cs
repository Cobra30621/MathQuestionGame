using NueGames.Action;
using NueGames.Managers;

namespace CardAction
{
    public class UnlimitedResources : CardActionBase
    {
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new DrawCardAction(
                999, GetActionSource()));

            GameActionExecutor.AddToBottom(new EarnManaAction(
                999, GetActionSource()));
        }
    }
}