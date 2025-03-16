using Effect.Card;
using Power;

namespace Effect.Condition
{
    /// <summary>
    /// 獲得 [1] 點格檔，符合條件 [2] 時 ，多獲得 [2] 點格檔
    /// </summary>
    public class AddMoreBlockWhenConditionEffect : EffectBase
    {
        private JudgeCondition _condition;
        private int defalutAmount;
        private int addition;
        
        public AddMoreBlockWhenConditionEffect(int defalutAmount, JudgeCondition condition, int addition)
        {
            this.defalutAmount = defalutAmount;
            _condition = condition;
            this.addition = addition;
        }

        public AddMoreBlockWhenConditionEffect(SkillInfo skillInfo)
        {
            defalutAmount = skillInfo.EffectParameterList[0];
            _condition = (JudgeCondition)skillInfo.EffectParameterList[1];
            addition = skillInfo.EffectParameterList[2];
        }
        
        
        public override void Play()
        {
            var addAmount = defalutAmount;
            if (ConditionChecker.PassCondition(_condition))
            {
                addAmount += addition;
            }
            
            foreach (var target in TargetList)
            {
                target.ApplyPower(PowerName.Block, addAmount, EffectSource);
            }
        }
    }
}