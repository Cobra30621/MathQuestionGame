using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private MMF_Player onEnterQuestionModeFeedback;
        [SerializeField] private MMF_Player onQuestionShowFeedback;
        [SerializeField] private MMF_Player onAnswerFeedback;
        [SerializeField] private MMF_Player onExitQuestionModeFeedback;
        
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject mainPanel;
        
        private static readonly int InQuestioningMode = Animator.StringToHash("In Questioning Mode");
        private static readonly int ShowQuestion = Animator.StringToHash("Show Question");
        private static readonly int OnAnswerQuestion = Animator.StringToHash("On Answer");

        void Awake()
        {
            // DisablePanel();
        }

        void Update()
        {
            // timeText.text = $"{Mathf.Ceil(_questionManager.Timer)}";
            // float timeRate = _questionManager.Timer / _questionManager.StartTime;
            // timeBar.fillAmount = timeRate;

            if (QuestionManager.Instance.IsQuestioning)
            {
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            // answerBar.UpdateUI(_questionManager.HasAnswerCount, _questionManager.Parameters.QuestionCount);
            int questionCount = QuestionManager.Instance.QuestionCount;
            int leastQuestionCount = questionCount - QuestionManager.Instance.HasAnswerCount;
            needAnswerCount.text = $"{leastQuestionCount}";
        }
        

        public void EnterQuestionMode()
        {
            mainPanel.SetActive(true);
            StartPlayAnimation();
            animator.SetBool(InQuestioningMode, true);
            // onEnterQuestionModeFeedback.PlayFeedbacks();
        }

        public void ExitQuestionMode(string info)
        {
            infoText.text = info;
            StartPlayAnimation();
            animator.SetBool(InQuestioningMode, false);
            // onExitQuestionModeFeedback.PlayFeedbacks();
        }

        public void SetNextQuestion(Question question)
        {
            qeustionImage.sprite = question.QuestionSprite;
            // optionImage.sprite = question.OptionSprite;
            
            StartPlayAnimation();
            animator.SetTrigger(ShowQuestion);
            // onQuestionShowFeedback.PlayFeedbacks();
        }
        

        public void OnAnswer(bool correct, int option)
        {
            StartPlayAnimation();
            animator.SetTrigger(OnAnswerQuestion);
            QuestionManager.Instance.GetAnswerButton(option)?.PlayOnAnswerAnimation(correct);
            
            // onAnswerFeedback.GetFeedbackOfType<MMF_Feedbacks>().TargetFeedbacks =
            //     _questionManager.GetAnswerFeedback(correct, option);
            // onAnswerFeedback.PlayFeedbacks();
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

