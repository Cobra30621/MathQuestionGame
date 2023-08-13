using System.Collections.Generic;

namespace Question
{
    /// <summary>
    /// 取的數學題目的參數
    /// </summary>
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
    }
}