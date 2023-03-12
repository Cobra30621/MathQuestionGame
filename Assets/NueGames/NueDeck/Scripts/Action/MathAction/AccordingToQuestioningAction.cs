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
        private CardActionParameters cardParameters;
        
        public AccordingToQuestioningAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameters cardActionParameters)
        {
            cardParameters = cardActionParameters;
            additionValue = cardActionParameters.CardActionData.AdditionValue;
            baseValue = cardActionParameters.Value;
        }

        public override void DoAction()
        {
            int correctAnswerCount = QuestionManager.Instance.CorrectAnswerCount;
            
            Value = baseValue + correctAnswerCount * additionValue;
            CardActionParameters newParameters = new CardActionParameters(
                Value, 
                cardParameters.TargetCharacter, 
                cardParameters.SelfCharacter, 
                cardParameters.CardActionData, 
                cardParameters.CardData
            );
            GameActionBase gameActionBase = GameActionManager.GetGameAction(afterQuestioningAction, newParameters);
            GameActionManager.AddToBottom(gameActionBase);
        }
    }
}