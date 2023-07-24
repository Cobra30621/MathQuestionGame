using Action.Parameters;
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
        public override ActionName ActionName => ActionName.EnterMathQuestioning;
        private MathQuestioningActionParameters mathParameters;

        public override void SetValue(ActionParameters parameters)
        {
            base.SetValue(parameters);
            
            // TODO Add Math Action
            // CardData cardData = parameters.CardData;
            //
            // mathParameters = cardData.MathQuestioningActionParameters;
            // mathParameters.TargetCharacter = parameters.TargetList;
            // mathParameters.SelfCharacter  = parameters.ActionSource.SourceCharacter;
            
            // mathParameters.LimitedQuestionAction = GameActionGenerator.GetGameActions(cardData, 
            //     parameters.SourceType, cardData.LimitedQuestionCardActionDataList, Self, TargetList);
            // mathParameters.CorrectActions = GameActionGenerator.GetGameActions(cardData, 
            //     parameters.SourceType, cardData.CorrectCardActionDataList , Self, TargetList);
            // mathParameters.WrongActions = GameActionGenerator.GetGameActions(cardData, 
            //     parameters.SourceType, cardData.WrongCardActionDataList, Self, TargetList);
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