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
        
        public override void SetValue(CardActionParameters cardActionParameters)
        {
            CardData cardData = cardActionParameters.CardData;
            Duration = cardActionParameters.CardActionData.ActionDelay;
            Target = cardActionParameters.TargetCharacter;
            Self = cardActionParameters.SelfCharacter;

            parameters = cardData.MathQuestioningActionParameters;
            parameters.TargetCharacter = cardActionParameters.TargetCharacter;
            parameters.SelfCharacter  = cardActionParameters.SelfCharacter;
            parameters.LimitedQuestionAction = GameActionManager.GetGameActions(cardData, cardData.LimitedQuestionCardActionDataList, Self, Target);
            parameters.CorrectActions = GameActionManager.GetGameActions(cardData, cardData.CorrectCardActionDataList , Self, Target);
            parameters.WrongActions = GameActionManager.GetGameActions(cardData, cardData.WrongCardActionDataList, Self, Target);
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