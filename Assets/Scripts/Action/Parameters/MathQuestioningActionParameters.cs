using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using Question;
using Question.QuestionAction;

namespace NueGames.Action.MathAction
{
   

    public class QuestionActionParameters
    {
        public int QuestionCount;
        
        /// <summary>
        /// 需要答對的題數
        /// </summary>
        public int NeedAnswerCount;

        public QuestionActionBase QuestionAction;
        
        
    }

    public class QuestionSetting
    {
        public int Grade;
        public MathType MathType; 
    }

    
}