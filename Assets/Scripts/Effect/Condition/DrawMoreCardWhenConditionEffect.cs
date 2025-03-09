using Effect.Card;
using UnityEngine;

namespace Effect.Condition
{
    /// <summary>
    /// 從牌組抽 [1] 張卡，符合條件 [2] 時 ，多抽[3]張卡
    /// </summary>
    public class DrawMoreCardWhenConditionEffect : EffectBase
    {
        private JudgeCondition _condition;
        private int defalutAmount;
        private int addition;

        public DrawMoreCardWhenConditionEffect(SkillInfo skillInfo)
        {
            defalutAmount = skillInfo.EffectParameterList[0];
            _condition = (JudgeCondition)skillInfo.EffectParameterList[1];
            addition = skillInfo.EffectParameterList[2];
        }
        
        
        public override void Play()
        {
            var drawCount = defalutAmount;
            if (ConditionChecker.PassCondition(_condition))
            {
                drawCount += addition;
            }
            
            Debug.Log($"抽卡 {drawCount}");
            CollectionManager.DrawCards(drawCount);
        }
    }
}