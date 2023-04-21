using System.Collections;
using System.Collections.Generic;
using Question;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



namespace Question
{
    public class MathActionBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressBarForeground;
        // private QuestionManager QuestionManager = QuestionManager.Instance;
        
        public void UpdateUI(int answerCount, int needAnswerCount)
        {
            progressText.text = $"{answerCount} / {needAnswerCount}";
            progressBarForeground.fillAmount = (float) answerCount / needAnswerCount;
        }
    }
}

