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
        private MathQuestioningActionParameters mathParameters;

        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            QuestionManager.Instance.EnterQuestionMode(mathParameters);
        }
    }
    
    
}