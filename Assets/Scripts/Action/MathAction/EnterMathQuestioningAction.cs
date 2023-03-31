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
        private MathQuestioningActionParameters parameters;
        public EnterMathQuestioningAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(ActionParameters parameters)
        {
            CardData cardData = parameters.CardData;
            Duration = parameters.ActionData.ActionDelay;
            Target = parameters.TargetCharacter;
            Self = parameters.SelfCharacter;

            this.parameters = cardData.MathQuestioningActionParameters;
            this.parameters.TargetCharacter = parameters.TargetCharacter;
            this.parameters.SelfCharacter  = parameters.SelfCharacter;
            this.parameters.LimitedQuestionAction = GameActionGenerator.GetGameActions(cardData, cardData.LimitedQuestionCardActionDataList, Self, Target);
            this.parameters.CorrectActions = GameActionGenerator.GetGameActions(cardData, cardData.CorrectCardActionDataList , Self, Target);
            this.parameters.WrongActions = GameActionGenerator.GetGameActions(cardData, cardData.WrongCardActionDataList, Self, Target);
        }

        public void SetValue(MathQuestioningActionParameters newParameters)
        {
            parameters = newParameters;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            Debug.Log("parameters.QuestionCount" + parameters.QuestionCount);
            QuestionManager.Instance.EnterQuestionMode(parameters);

            PlayAudio();
        }
    }
    
    
}