using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using Question;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
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