using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

namespace Question
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