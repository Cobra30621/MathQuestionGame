using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;

namespace Action.Enemy
{
    /// <summary>
    /// 直接設定敵人死亡
    /// </summary>
    public class SetDeathAction : GameActionBase
    {

        public SetDeathAction(List<CharacterBase> targets, ActionSource actionSource)
        {
            TargetList = targets;
            ActionSource = actionSource;
        }
        
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.SetDeath();
            }
        }
    }
}