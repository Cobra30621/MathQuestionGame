using GameListener;
using NueGames.Enums;

namespace NueGames.Power.Math
{
    /// <summary>
    /// 答錯數學題目時，給予的傷害增加 50 %
    /// </summary>
    public class MathAngryPower : PowerBase
    {
        public override PowerName PowerName => PowerName.MathAngry;

        public MathAngryPower()
        {
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }

        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerWrong += OnAnswerWrong;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerWrong -= OnAnswerWrong;
        }


        public override float AtDamageGive(float i)
        {
            return i * (1 + (Amount - 1) * 0.2f);
        }


        protected override void OnAnswerWrong()
        {
            StackPower(1);
        }
    }
}