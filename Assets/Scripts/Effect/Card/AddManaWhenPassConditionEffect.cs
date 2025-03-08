using Sirenix.OdinInspector;
using UnityEngine;

namespace Effect.Card
{
    /// <summary>
    /// 當符合條件時，增加魔力
    /// </summary>
    public class AddManaWhenPassConditionEffect : EffectBase
    {
        private JudgeCondition _condition;
        private int _manaAmount;

        public AddManaWhenPassConditionEffect(SkillInfo skillInfo)
        {
            _condition = (JudgeCondition)skillInfo.EffectParameterList[0];
            _manaAmount = skillInfo.EffectParameterList[1];
        }
        
        
        public override void Play()
        {
            if (PassCondition())
            {
                CombatManager.AddMana(_manaAmount);
            }
        }


        private bool PassCondition()
        {
            switch (_condition)
            {
                case JudgeCondition.FirstCardInThisTurn:
                    return CombatManager.CombatCounter.IsFirstUseCardInCurrentTurn();
            }

            return true;
        }
        
        
    }


    public enum JudgeCondition
    {
        [LabelText("本回合打出的第一張卡牌")]
        FirstCardInThisTurn = 1,
    }
}