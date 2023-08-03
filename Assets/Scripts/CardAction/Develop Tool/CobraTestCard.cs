using NueGames.Action.MathAction;
using NueGames.Managers;

namespace CardAction
{
    public class CobraTestCard : CardActionBase
    {
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new AddSkillCountAction(1));
        }
    }
}