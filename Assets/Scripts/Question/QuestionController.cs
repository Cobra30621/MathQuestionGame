using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Question
{
    public class QuestionController : MonoBehaviour
    {
        private QuestionManager _questionManager;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private Image qeustionImage;
        [SerializeField] private Image timeBar;
        [SerializeField] private Image correctBar;
        [SerializeField] private  TextMeshProUGUI timeText;
        [SerializeField] private  TextMeshProUGUI wrongCountText;
        [SerializeField] private TextMeshProUGUI correctCountText;

        void Awake()
        {
            DisablePanel();
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

        public void ShowPanel()
        {
            mainPanel.SetActive(true);
        }

        public void DisablePanel()
        {
            mainPanel.SetActive(false);
        }
        
        public void SetNextQuestion(MultipleChoiceQuestion multipleChoiceQuestion)
        {
            qeustionImage.sprite = multipleChoiceQuestion.QuestionSprite;
        }

        public void OnAnswer(bool correct)
        {
            if (correct)
            {
                Debug.Log("Play Correct Effect");
            }
            else
            {
                Debug.Log("Play Wrong Effect");
            }
        }
    }
}

