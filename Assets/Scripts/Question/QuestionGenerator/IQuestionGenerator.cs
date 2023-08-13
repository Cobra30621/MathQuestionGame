using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Question
{
    public abstract class IQuestionGenerator : SerializedMonoBehaviour
    {
        public abstract List<Question> GetQuestions(QuestionSetting request);
    }

    
}