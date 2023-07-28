using NueGames.Action;
using NueGames.Managers;

namespace CardAction
{
    public class UnlimitedResources : CardActionBase
    {
        protected override void DoMainAction()
        {
            var drawCardAction = new DrawCardAction();
            drawCardAction.SetValue(999, GetActionSource());
            GameActionExecutor.Instance.AddToBottom(drawCardAction);

            var earnManaAction = new EarnManaAction();
            drawCardAction.SetValue(999, GetActionSource());
            GameActionExecutor.Instance.AddToBottom(earnManaAction);
        }
    }
}