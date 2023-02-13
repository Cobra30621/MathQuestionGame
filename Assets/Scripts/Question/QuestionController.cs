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
        public Image qeustionImage;
        public Image timeBar;
        public Image correctBar;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI wrongCountText;

        public TextMeshProUGUI correctCountText;

        void Update()
        {
            timeText.text = $"{Mathf.Ceil(_questionManager.Timer)}";
            float timeRate = _questionManager.Timer / _questionManager.StartTime;
            timeBar.fillAmount = timeRate;

            correctCountText.text = $"{_questionManager.CorrectCount}";
            correctBar.fillAmount = (float)_questionManager.CorrectCount / _questionManager.NeedCorrectCount;
            // wrongCountText.text =  $"答錯 :{_questionManager.WrongCount}";
        }
        

        public void SetQuestionManager(QuestionManager manager)
        {
            _questionManager = manager;
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

