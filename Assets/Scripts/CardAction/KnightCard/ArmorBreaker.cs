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
        // protected void OnTurnStart(TurnInfo info)
        // {
        //     if (IsCharacterTurn(info))
        //     {
        //         
        //         GameActionExecutor.AddToBottom(new ApplyPowerAction(
        //             3, PowerName.Block, new List<CharacterBase>() {Owner},
        //             GetActionSource()));
        //     }
        // }
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                2, PowerName.Shield, TargetList, GetActionSource()));
        }
    }
}