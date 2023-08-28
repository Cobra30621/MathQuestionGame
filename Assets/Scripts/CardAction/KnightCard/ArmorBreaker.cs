using System.Collections.Generic;
using NueGames.Action;
using NueGames.Enums;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;
using NueGames.Power;
namespace CardAction.KnightCard
{
    public class ArmorBreaker : CardActionBase
    {
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                2, PowerName.Shield, TargetList, GetActionSource()));
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                1, PowerName.Bleed, TargetList, GetActionSource()));
        }
    }
}