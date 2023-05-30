using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using Question;
using UnityEngine;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 進入數學模式
    /// </summary>
    public class EnterMathQuestioningAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.EnterMathQuestioning;
        private MathQuestioningActionParameters mathParameters;

        public override void SetValue(ActionParameters parameters)
        {
            base.SetValue(parameters);
            
            CardData cardData = parameters.CardData;
            
            mathParameters = cardData.MathQuestioningActionParameters;
            mathParameters.TargetCharacter = parameters.TargetList;
            mathParameters.SelfCharacter  = parameters.Self;
            
            mathParameters.LimitedQuestionAction = GameActionGenerator.GetGameActions(cardData, 
                parameters.ActionSource, cardData.LimitedQuestionCardActionDataList, Self, TargetList);
            mathParameters.CorrectActions = GameActionGenerator.GetGameActions(cardData, 
                parameters.ActionSource, cardData.CorrectCardActionDataList , Self, TargetList);
            mathParameters.WrongActions = GameActionGenerator.GetGameActions(cardData, 
                parameters.ActionSource, cardData.WrongCardActionDataList, Self, TargetList);
        }

        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            QuestionManager.Instance.EnterQuestionMode(mathParameters);
        }
    }
    
    
}