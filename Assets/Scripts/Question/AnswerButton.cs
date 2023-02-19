using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

namespace Question
{
    public class AnswerButton : MonoBehaviour
    {
        public MMF_Player CorrectFeedback => correctFeedback;
        [SerializeField] private MMF_Player correctFeedback;
        public MMF_Player WrongFeedback => wrongFeedback;
        [SerializeField] private MMF_Player wrongFeedback;
        private Button button;

        void Awake()
        {
            button = GetComponent<Button>();
        }
        
        public void EnableAnswer(bool enable)
        {
            button.enabled = enable;
        }
    }
}