using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using Question;
using Question.QuestionAction;

namespace NueGames.Action.MathAction
{
    
    /// <summary>
    /// 數學題目的設定
    /// </summary>
    public class QuestionSetting
    {
        /// <summary>
        /// 年級
        /// </summary>
        public int Grade;
        /// <summary>
        /// 題目類型
        /// </summary>
        public MathType MathType; 
    }

    
}