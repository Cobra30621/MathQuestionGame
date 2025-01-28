using Question.Action;
using Question.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question.UI
{
    public class SelectedQuestionUI : MonoBehaviour
    {
        public int questionCount;
        
        [Button]
        public void StartQuestion()
        {
            var normalQuestionAction = new NormalQuestionAction()
            {
                QuestionCount = questionCount,
                NeedCorrectCount = 0,
                QuestionMode = QuestionMode.Easy
            };
            
            QuestionManager.Instance.EnterQuestionMode(normalQuestionAction);
        }
    }
}