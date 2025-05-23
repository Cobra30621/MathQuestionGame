using System.Collections.Generic;
using Map;
using Question.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question.QuestionGenerate
{
    /// <summary>
    /// 讀取本地端的數學題目(Demo 版用)
    /// </summary>
    public class LocalQuestionGetter : SerializedMonoBehaviour
    {
        [SerializeField] private QuestionData questionData;
        /// <summary>
        /// 取得數學題目清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<Data.Question> GetQuestions(QuestionSetting request)
        {
            var questions = new List<Data.Question>();
            questions.AddRange(questionData.GetQuestion(request.Publisher, request.Grade));

            questions.Shuffle();

            return questions;
        }
    }
}