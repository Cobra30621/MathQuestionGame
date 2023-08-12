using System.Collections.Generic;

namespace Question
{
    /// <summary>
    /// 取的數學題目的參數
    /// </summary>
    public class GetQuestionsRequest
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
        /// 生成題目的數量
        /// </summary>
        public int GetCount;


        public GetQuestionsRequest(List<Grade> grades, List<Publisher> publishers, int getCount)
        {
            Grades = grades;
            Publishers = publishers;
            GetCount = getCount;
        }

        public GetQuestionsRequest(Grade grade, Publisher publisher, int getCount)
        {
            Grades = new List<Grade>() { grade };
            Publishers = new List<Publisher>() { publisher };
            GetCount = getCount;
        }
    }
}