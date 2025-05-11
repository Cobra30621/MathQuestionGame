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
        public List<Grade> Grades;
        /// <summary>
        /// 出版社清單
        /// </summary>
        public List<Publisher> Publishers;
        
        /// <summary>
        /// 需要回答的次數
        /// </summary>
        public int needAnswerCount;

        public QuestionSetting()
        {
            Grades = new List<Grade>();
            Publishers = new List<Publisher>();
        }

        public QuestionSetting(List<Grade> grades, List<Publisher> publishers)
        {
            Grades = grades;
            Publishers = publishers;
        }

        public QuestionSetting(Grade grade, Publisher publisher)
        {
            Grades = new List<Grade>() { grade };
            Publishers = new List<Publisher>() { publisher };
        }

        public override string ToString()
        {
            return $"{nameof(Grades)}: {Grades.Count}, {nameof(Publishers)}: {Publishers.Count}";
        }
    }
}