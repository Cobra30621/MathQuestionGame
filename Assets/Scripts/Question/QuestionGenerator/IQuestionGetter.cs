using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Question
{
    public abstract class IQuestionGetter : SerializedMonoBehaviour
    {
        public abstract List<Question> GetQuestions(QuestionSetting request);

        public abstract bool EnableGetQuestion();
    }

    
}