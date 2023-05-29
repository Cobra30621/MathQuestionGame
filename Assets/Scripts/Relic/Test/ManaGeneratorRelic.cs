using Action.Power;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;
using NueGames.Parameters;

namespace NueGames.Relic.Common
{
    /// <summary>
    /// 答對一題，獲得數學瑪娜
    /// </summary>
    public class ManaGeneratorRelic : RelicBase
    {
        public override RelicType RelicType => RelicType.ManaGenerator;


        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect += OnAnswerCorrect;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect -= OnAnswerCorrect;
        }
        
        protected override void OnAnswerCorrect()
        {
            var applyMathManaAction = new ApplyMathManaAction();
            var parameter = new ApplyPowerParameters(CombatManager.Instance.CurrentMainAlly,
                PowerType.MathMana, 1);
            applyMathManaAction.SetValue(parameter);
            GameActionExecutor.Instance.AddToBottom(applyMathManaAction);
        }
        
    }
}