using NueGames.Enums;

namespace NueGames.Power.Math
{
    /// <summary>
    /// 答錯數學題目時，給予的傷害增加 50 %
    /// </summary>
    public class MathAngryPower : PowerBase
    {
        public override PowerType PowerType => PowerType.MathAngry;

        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerWrong += OnAnswerWrong;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerWrong -= OnAnswerWrong;
        }


        public override float AtDamageGive(float damage)
        {
            return damage * (1 + (Amount - 1) * 0.2f);
        }


        protected override void OnAnswerWrong()
        {
            StackPower(1);
        }
    }
}