using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using Question;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class AccordingToQuestioningAction : GameActionBase
    {
        private int baseValue;
        private int additionValue;
        private GameActionType afterQuestioningAction;
        private CardActionParameter cardParameter;
        
        public AccordingToQuestioningAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameter cardActionParameter)
        {
            cardParameter = cardActionParameter;
            additionValue = cardActionParameter.CardActionData.AdditionValue;
            baseValue = cardActionParameter.Value;
        }

        public override void DoAction()
        {
            int correctAnswerCount = QuestionManager.Instance.CorrectAnswerCount;
            
            Value = baseValue + correctAnswerCount * additionValue;
            CardActionParameter newParameter = new CardActionParameter(
                Value, 
                cardParameter.TargetCharacter, 
                cardParameter.SelfCharacter, 
                cardParameter.CardActionData, 
                cardParameter.CardData
            );
            GameActionBase gameActionBase = GameActionManager.GetGameAction(afterQuestioningAction, newParameter);
            GameActionManager.AddToBottom(gameActionBase);
        }
    }
}