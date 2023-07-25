using System.Collections.Generic;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;

namespace CardAction.Common
{
    public class DamageCard : CardActionBase
    {
        public int damageValue;
        public override void DoAction()
        {
            DamageAction damageAction = new DamageAction();
            
            damageAction.SetDamageActionValue(damageValue, TargetList, GetActionSource());
            damageAction.SetFXValue(FxName.TearAttack, FxSpawnPosition.EachTarget);
            
            GameActionExecutor.Instance.AddToBottom(damageAction);
        }
    }
}