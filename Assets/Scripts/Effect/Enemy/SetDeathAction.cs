using System.Collections.Generic;
using Characters;
using Effect.Parameters;

namespace Effect.Enemy
{
    /// <summary>
    /// 直接設定敵人死亡
    /// </summary>
    public class SetDeathAction : EffectBase
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