using Effect.Card;

namespace Effect.Condition
{
    /// <summary>
    /// 當符合 [1] 條件時，增加 [2] 點魔力
    /// </summary>
    public class AddManaWhenConditionEffect : EffectBase
    {
        private JudgeCondition _condition;
        private int _manaAmount;

        public AddManaWhenConditionEffect(SkillInfo skillInfo)
        {
            _condition = (JudgeCondition)skillInfo.EffectParameterList[0];
            _manaAmount = skillInfo.EffectParameterList[1];
        }
        
        
        public override void Play()
        {
            if (ConditionChecker.PassCondition(_condition))
            {
                CombatManager.AddMana(_manaAmount);
            }
        }
        
    }
}