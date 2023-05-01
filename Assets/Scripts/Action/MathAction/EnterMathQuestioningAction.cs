using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
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
        public EnterMathQuestioningAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(ActionParameters parameters)
        {
            base.SetValue(parameters);
            
            CardData cardData = parameters.CardData;
            
            mathParameters = cardData.MathQuestioningActionParameters;
            mathParameters.TargetCharacter = parameters.Target;
            mathParameters.SelfCharacter  = parameters.Self;
            
            mathParameters.LimitedQuestionAction = GameActionGenerator.GetGameActions(cardData, 
                parameters.ActionSource, cardData.LimitedQuestionCardActionDataList, Self, Target);
            mathParameters.CorrectActions = GameActionGenerator.GetGameActions(cardData, 
                parameters.ActionSource, cardData.CorrectCardActionDataList , Self, Target);
            mathParameters.WrongActions = GameActionGenerator.GetGameActions(cardData, 
                parameters.ActionSource, cardData.WrongCardActionDataList, Self, Target);
        }

        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            Debug.Log("parameters.QuestionCount" + mathParameters.QuestionCount);
            QuestionManager.Instance.EnterQuestionMode(mathParameters);

            PlayAudio();
        }
    }
    
    
}