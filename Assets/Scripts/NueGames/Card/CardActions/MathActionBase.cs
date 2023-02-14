using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using Question;
using UnityEngine;
namespace NueGames.Card.CardActions

{
    public abstract class MathActionBase: CardActionBase
    {
        public override CardActionType ActionType => CardActionType.Math;
        protected QuestionManager QuestionManager => QuestionManager.Instance;
        private CardActionParameters actionParameters;

        public override void DoAction(CardActionParameters actionParameters)
        {
            QuestionManager.Instance.EnterQuestion(this);
            this.actionParameters = actionParameters;
            
        }
        

        public void OnAnswer(bool correct)
        {
            if (correct)
            {
                actionParameters.CardBase.PlayMathCardAction(actionParameters.CardData.CorrectCardActionDataList);
            }
            else
            {
                actionParameters.CardBase.PlayMathCardAction(actionParameters.CardData.WrongCardActionDataList);
            }

        }
        
    }
}