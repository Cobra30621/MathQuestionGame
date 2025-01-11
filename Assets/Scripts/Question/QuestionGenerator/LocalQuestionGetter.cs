using System.Collections.Generic;
using Map;
using Question.Data;
using UnityEngine;

namespace Question.QuestionGenerator
{
    /// <summary>
    /// 讀取本地端的數學題目(Demo 版用)
    /// </summary>
    public class LocalQuestionGetter : IQuestionGetter
    {
        [SerializeField] private QuestionData questionData;
        /// <summary>
        /// 取得數學題目清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override List<Data.Question> GetQuestions(QuestionSetting request)
        {
            var questions = new List<Data.Question>();
            foreach (var publisher in request.Publishers)
            {
                foreach (var grade in request.Grades)
                {
                    questions.AddRange(questionData.GetQuestion(publisher, grade));
                }
            }

            questions.Shuffle();

            return questions;
        }

        public override bool EnableGetQuestion()
        {
            return true;
        }
    }
}