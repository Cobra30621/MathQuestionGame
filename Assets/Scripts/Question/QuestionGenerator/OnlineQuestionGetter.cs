using System.Collections.Generic;
using Question.QuestionLoader;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question
{
    public class OnlineQuestionGetter : IQuestionGetter
    {
        [Required] [SerializeField]
        private OnlineQuestionDownloader onlineQuestionDownloader;

        [LabelText("最少需要的題目數")]
        public int minNeedQuestionCount = 5;
        
        
        public override List<Question> GetQuestions(QuestionSetting request)
        {
            return onlineQuestionDownloader.questions;
        }

        public override bool EnableGetQuestion()
        {
            return onlineQuestionDownloader.questions.Count >= minNeedQuestionCount;
        }
    }
}