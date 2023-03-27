using Question;

namespace NueGames.Action.MathAction
{
    public abstract class ByQuestioningActionBase : GameActionBase
    {
        protected int baseValue;
        protected int additionValue;
        
        protected int GetAddedValue()
        {
            int correctAnswerCount = QuestionManager.Instance.CorrectAnswerCount;
            
            return baseValue + correctAnswerCount * additionValue;
        }
    }
}