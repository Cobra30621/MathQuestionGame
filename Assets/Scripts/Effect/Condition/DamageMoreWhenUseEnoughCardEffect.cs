using Effect.Damage;
using Effect.Parameters;

namespace Effect.Condition
{
    /// <summary>
    /// 造成 [1] 點傷害，如果本回合使用過 [2] 張以上的卡片，多造成 [3] 點傷害
    /// </summary>
    public class DamageMoreWhenUseEnoughCardEffect : EffectBase
    {
        private int defalutAmount;
        private int needUseCard;
        private int addition;

        public DamageMoreWhenUseEnoughCardEffect(SkillInfo skillInfo)
        {
            defalutAmount = skillInfo.EffectParameterList[0];
            needUseCard = skillInfo.EffectParameterList[1];
            addition = skillInfo.EffectParameterList[2];
        }
        
        
        public override void Play()
        {
            var addAmount = defalutAmount;
            if (ConditionChecker.UseEnoughCard(needUseCard))
            {
                addAmount += addition;
            }
            
            var damageInfo = new DamageInfo(addAmount, EffectSource);
            var damageAction = new DamageEffect(damageInfo,  TargetList);
            damageAction.Play();
        }
    }
}