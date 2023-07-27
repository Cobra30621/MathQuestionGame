using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using Question;

namespace NueGames.Action.MathAction
{
   

    public class QuestionActionParameters
    {
        public int QuestionCount;
        
        /// <summary>
        /// 需要答對的題數
        /// </summary>
        public int CorrectActionNeedAnswerCount;
        /// <summary>
        /// 答對題數達標發動的效果
        /// </summary>
        public List<ActionData> CorrectActions;

        /// <summary>
        /// 需要答錯的題數
        /// </summary>
        public int WrongActionNeedAnswerCount;
        /// <summary>
        /// 答錯題數達標發動的效果
        /// </summary>
        public List<GameActionBase> WrongActions;
    }

    public class QuestionSetting
    {
        public int Grade;
        public MathType MathType; 
    }

    
}