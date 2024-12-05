using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueExtentions;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Question
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
        public override List<Question> GetQuestions(QuestionSetting request)
        {
            var questions = new List<Question>();
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