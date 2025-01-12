using UnityEngine;

namespace Effect.Common
{
    /// <summary>
    /// 當找不到效果時，給予的空效果
    /// </summary>
    public class NullEffect : EffectBase
    {

        public NullEffect(SkillInfo skillInfo)
        {
            
        }

        public override void Play()
        {
        }
    }
}