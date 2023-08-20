using Action.Parameters;
using NueGames.Action;
using NueGames.Managers;
using NueGames.Power;

namespace CardAction.KnightCard
{
    public class ShieldBash : CardActionBase
    {
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new DamageByAllyPowerValueAction(TargetList, PowerName.Block, GetActionSource(), 1, false, false));
        }
    }
}