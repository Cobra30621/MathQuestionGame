using MoreMountains.Feedbacks;
using UnityEngine;

namespace Question.Answer_Button
{
    public class AnswerButtonUseMMFPlayer : AnswerButtonBase
    {
        public MMF_Player CorrectFeedback => correctFeedback;
        [SerializeField] private MMF_Player correctFeedback;
        public MMF_Player WrongFeedback => wrongFeedback;
        [SerializeField] private MMF_Player wrongFeedback;
        public override void PlayOnAnswerAnimation(bool isCorrect)
        {
            throw new System.NotImplementedException();
        }
    }
}