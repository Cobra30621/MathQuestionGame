using Question;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 根據答對數，產生不同行動
    /// </summary>
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