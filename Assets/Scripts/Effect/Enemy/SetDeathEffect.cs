using System.Collections.Generic;
using Characters;
using Effect.Parameters;

namespace Effect.Enemy
{
    /// <summary>
    /// 直接設定敵人死亡
    /// </summary>
    public class SetDeathEffect : EffectBase
    {

        public SetDeathEffect(List<CharacterBase> targets, EffectSource effectSource)
        {
            TargetList = targets;
            EffectSource = effectSource;
        }

        public override void Play()
        {
            foreach (var target in TargetList)
            {
                target.SetDeath();
            }
        }
    }
}