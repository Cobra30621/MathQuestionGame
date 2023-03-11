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

        [SerializeField] private TextMeshProUGUI actionContent;
    
        [SerializeField] private Image progressBarForeground;
        public QuestionManager QuestionManager = QuestionManager.Instance;


        public void UpdateUI()
        {
        
        }
    
    }

}

