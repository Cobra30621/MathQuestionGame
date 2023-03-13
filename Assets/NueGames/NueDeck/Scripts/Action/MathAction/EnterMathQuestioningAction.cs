using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using Question;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class EnterMathQuestioningAction : GameActionBase
    {
        private MathQuestioningActionParameters parameters;
        public GameActionManager GameActionManager => GameActionManager.Instance;
        
        public EnterMathQuestioningAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameters parameters)
        {
            CardData cardData = parameters.CardData;
            Duration = parameters.CardActionData.ActionDelay;
            Target = parameters.TargetCharacter;
            Self = parameters.SelfCharacter;

            this.parameters = cardData.MathQuestioningActionParameters;
            this.parameters.TargetCharacter = parameters.TargetCharacter;
            this.parameters.SelfCharacter  = parameters.SelfCharacter;
            this.parameters.LimitedQuestionAction = GameActionManager.GetGameActions(cardData, cardData.LimitedQuestionCardActionDataList, Self, Target);
            this.parameters.CorrectActions = GameActionManager.GetGameActions(cardData, cardData.CorrectCardActionDataList , Self, Target);
            this.parameters.WrongActions = GameActionManager.GetGameActions(cardData, cardData.WrongCardActionDataList, Self, Target);
        }

        public void SetValue(MathQuestioningActionParameters newParameters)
        {
            parameters = newParameters;
        }
        
        public override void DoAction()
        {
            Debug.Log("parameters.QuestionCount" + parameters.QuestionCount);
            QuestionManager.Instance.EnterQuestionMode(parameters);

            PlayAudio();
        }
    }
    
    
}