using System;
using System.Collections.Generic;
using Question.Enum;

namespace Question.Data
{
    /// <summary>
    /// 取的數學題目的參數
    /// </summary>
    [Serializable]
    public class QuestionSetting
    {
        /// <summary>
        /// 年級清單
        /// </summary>
        public Grade Grade;
        /// <summary>
        /// 出版社清單
        /// </summary>
        public Publisher Publisher;
        
        /// <summary>
        /// 需要回答的次數
        /// </summary>
        public int needAnswerCount;





        public QuestionSetting(Grade grade, Publisher publisher)
        {
            Grade = grade ;
            Publisher= publisher ;
        }

        public override string ToString()
        {
            return $"{nameof(Grade)}: {Grade}, {Publisher}: {Publisher}";
        }
    }
}