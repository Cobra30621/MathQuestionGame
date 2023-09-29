using System.Collections;
using System.Collections.Generic;
using Feedback;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

namespace Question
{
    public class QuestionController : MonoBehaviour
    {
        [SerializeField] private Image qeustionImage;
        [SerializeField] private Image optionImage;

        [SerializeField] private TextMeshProUGUI needAnswerCount;

        
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private IFeedback onAnswerCorrectFeedback;
        [SerializeField] private IFeedback onAnswerWrongFeedback;
        
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject mainPanel;
        
        private static readonly int InQuestioningMode = Animator.StringToHash("In Questioning Mode");
        private static readonly int ShowQuestion = Animator.StringToHash("Show Question");
        private static readonly int OnAnswerQuestion = Animator.StringToHash("On Answer");

        void Update()
        {
            if (QuestionManager.Instance.IsQuestioning)
            {
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            int questionCount = QuestionManager.Instance.QuestionCount;
            int leastQuestionCount = questionCount - QuestionManager.Instance.HasAnswerCount;
            needAnswerCount.text = $"{leastQuestionCount}";
        }
        

        public void EnterQuestionMode()
        {
            mainPanel.SetActive(true);
            StartPlayAnimation();
            animator.SetBool(InQuestioningMode, true);
        }

        public void ExitQuestionMode(string info)
        {
            infoText.text = info;
            StartPlayAnimation();
            animator.SetBool(InQuestioningMode, false);
        }

        public void SetNextQuestion(Question question)
        {
            qeustionImage.sprite = question.QuestionSprite;
            optionImage.sprite = question.OptionSprite;
            
            StartPlayAnimation();
            animator.SetTrigger(ShowQuestion);
        }
        

        public void OnAnswer(bool correct, int option)
        {
            StartPlayAnimation();
            animator.SetTrigger(OnAnswerQuestion);
            QuestionManager.Instance.GetAnswerButton(option)?.PlayOnAnswerAnimation(correct);

            if (correct)
            {
                onAnswerCorrectFeedback.Play();
            }
            else
            {
                onAnswerWrongFeedback.Play();
            }
        }

        public void ClosePanel()
        {
            mainPanel.SetActive(false);
        }
        
        public void StartPlayAnimation()
        {
            QuestionManager.Instance.StartPlayAnimation();
        }
    }
}

