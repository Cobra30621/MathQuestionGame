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
        private QuestionManager _questionManager;
        [SerializeField] private Image qeustionImage;
        [SerializeField] private Image timeBar;
        [SerializeField] private Image correctBar;
        [SerializeField] private  TextMeshProUGUI timeText;
        [SerializeField] private  TextMeshProUGUI wrongCountText;
        [SerializeField] private TextMeshProUGUI correctCountText;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private MMF_Player onEnterQuestionModeFeedback;
        [SerializeField] private MMF_Player onQuestionShowFeedback;
        [SerializeField] private MMF_Player onAnswerFeedback;
        [SerializeField] private MMF_Player onExitQuestionModeFeedback;

        void Awake()
        {
            // DisablePanel();
        }

        void Update()
        {
            // timeText.text = $"{Mathf.Ceil(_questionManager.Timer)}";
            // float timeRate = _questionManager.Timer / _questionManager.StartTime;
            // timeBar.fillAmount = timeRate;

            correctCountText.text = $"答對：{_questionManager.CorrectCount}/ {_questionManager.CorrectActionNeedAnswerCount}";
            correctBar.fillAmount = (float)_questionManager.CorrectCount / _questionManager.CorrectActionNeedAnswerCount;
            wrongCountText.text =  $"答錯 :{_questionManager.WrongCount} / {_questionManager.WrongActionNeedAnswerCount}";
        }
        

        public void SetQuestionManager(QuestionManager manager)
        {
            _questionManager = manager;
        }

        public void EnterQuestionMode()
        {
            
            // mainPanel.SetActive(true);
            onEnterQuestionModeFeedback.PlayFeedbacks();
        }

        public void ExitQuestionMode(bool correct)
        {
            if (correct)
            {
                infoText.text = "魔法詠唱成功，發動好效果";
            }else
            {
                infoText.text = "魔法詠唱失敗，發動壞效果";
            }
            onExitQuestionModeFeedback.PlayFeedbacks();
        }
        
        public void SetNextQuestion(MultipleChoiceQuestion multipleChoiceQuestion)
        {
            qeustionImage.sprite = multipleChoiceQuestion.QuestionSprite;
            onQuestionShowFeedback.PlayFeedbacks();
        }
        

        public void OnAnswer(bool correct, int option)
        {
            onAnswerFeedback.GetFeedbackOfType<MMF_Feedbacks>().TargetFeedbacks =
                _questionManager.GetAnswerFeedback(correct, option);
            onAnswerFeedback.PlayFeedbacks();
        }
    }
}

