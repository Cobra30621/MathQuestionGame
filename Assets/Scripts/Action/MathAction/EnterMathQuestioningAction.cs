using Action.Parameters;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using Question;
using Question.QuestionAction;
using UnityEngine;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 進入數學模式
    /// </summary>
    public class EnterMathQuestioningAction : GameActionBase
    {
        private readonly QuestionActionBase _questionAction;
        private QuestionSetting _questionSetting;

        public EnterMathQuestioningAction(QuestionActionBase questionAction, QuestionSetting questionSetting)
        {
            _questionAction = questionAction;
            _questionSetting = questionSetting;
        }


        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            QuestionManager.Instance.EnterQuestionMode(_questionAction, _questionSetting);
        }
    }
    
    
}