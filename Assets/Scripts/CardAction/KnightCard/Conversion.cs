using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace CardAction.KnightCard
{
    public class Conversion : CardActionBase
    {
        protected override void DoMainAction()
        {
            int healValue = CombatManager.Instance.MainAlly.GetPowerValue(PowerName.Block);
            GameActionExecutor.AddToBottom(new HealAction(healValue,TargetList, GetActionSource()));
            GameActionExecutor.AddToBottom(new ClearPowerAction(PowerName.Block,TargetList, GetActionSource()));
        }
    }
}