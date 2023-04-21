using NueGames.Enums;
using Question;
using UnityEngine;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 根據答對數，產生不同行動
    /// </summary>
    public abstract class ByQuestioningActionBase : GameActionBase
    {
        protected int baseValue;
        protected int additionValue;
        protected int answerCount;
        protected AnswerOutcomeType _answerOutcomeType;
        
        protected int GetAddedValue()
        {
            // Debug.Log($"{base.ToString()}, {nameof(baseValue)}: {baseValue}, " +
            //           $"{nameof(additionValue)}: {additionValue}, {nameof(answerCount)}: {answerCount}, {nameof(_answerOutcomeType)}: {_answerOutcomeType}" +
            //           $"baseValue + answerCount * additionValue, {baseValue + answerCount * additionValue}");
            
            return baseValue + answerCount * additionValue;
        }

        
    }
}