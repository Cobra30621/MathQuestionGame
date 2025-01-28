using System.Collections.Generic;
using Question.Data;
using Question.QuestionLoader;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question.QuestionGenerate
{
    public class OnlineQuestionGetter : IQuestionGetter
    {
        [Required] [SerializeField]
        private OnlineQuestionDownloader onlineQuestionDownloader;

        [LabelText("最少需要的題目數")]
        public int minNeedQuestionCount = 5;
        
        
        public override List<Data.Question> GetQuestions(QuestionSetting request)
        {
            return onlineQuestionDownloader.questions;
        }

        public override bool EnableGetQuestion(QuestionSetting request)
        {
            return onlineQuestionDownloader.questions.Count >= minNeedQuestionCount;
        }
    }
}