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
            DamageAction damageAction = new DamageAction();
            damageAction.SetValue(damageValue, TargetList, GetActionSource());
            GameActionExecutor.Instance.AddToBottom(damageAction);
        }
    }
}