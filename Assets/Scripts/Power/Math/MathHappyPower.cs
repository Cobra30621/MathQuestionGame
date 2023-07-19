using GameListener;
using NueGames.Enums;

namespace NueGames.Power.Math
{
    /// <summary>
    /// 答對數學題目時，被打到的傷害增加 50 %
    /// </summary>
    public class MathHappyPower : PowerBase
    {
        public override PowerType PowerType => PowerType.MathHappy;

        public MathHappyPower()
        {
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }


        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect += OnAnswerCorrect;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect -= OnAnswerCorrect;
        }


        public override float AtDamageReceive(float damage)
        {
            return damage * (1 + (Amount - 1) * 0.2f);
        }

        protected override void OnAnswerCorrect()
        {
            StackPower(1);
        }
    }
}