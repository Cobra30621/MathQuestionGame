using System.Collections.Generic;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;

namespace CardAction
{
    public class Damage : CardActionBase
    {
        public int damageValue;
        protected override void DoMainAction()
        {
            GameActionExecutor.AddToBottom(new DamageAction(
                damageValue, TargetList, GetActionSource()));
        }

        // protected virtual int update()
        // {
        //     return damageValue;
        // }
    }
}