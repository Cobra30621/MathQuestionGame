using System.Collections.Generic;
using Characters;
using Effect.Parameters;

namespace Effect.Common
{
    /// <summary>
    /// 回血
    /// </summary>
    public class HealEffect : EffectBase
    {
        /// <summary>
        /// 回復血量
        /// </summary>
        private int _healValue;
        
        public HealEffect(int healValue, List<CharacterBase> targetList, EffectSource effectSource)
        {
            _healValue = healValue;
            TargetList = targetList;
            EffectSource = effectSource;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public HealEffect(SkillInfo skillInfo)
        {
            _healValue = skillInfo.EffectParameterList[0];
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void Play()
        {
            foreach (var target in TargetList)
            {
                target.Heal(_healValue);

                PlaySpawnTextFx($"{_healValue}", target.TextSpawnRoot);
            }
            
        }
    }
}