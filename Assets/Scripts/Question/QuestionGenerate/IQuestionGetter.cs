using System.Collections.Generic;
using Question.Data;
using Sirenix.OdinInspector;

namespace Question.QuestionGenerate
{
    public abstract class IQuestionGetter : SerializedMonoBehaviour
    {
        public abstract List<Data.Question> GetQuestions(QuestionSetting request);

        public abstract bool EnableGetQuestion(QuestionSetting request);
    }

    
}