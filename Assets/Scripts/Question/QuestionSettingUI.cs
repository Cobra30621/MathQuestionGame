using System;
using Question.Data;
using Question.Enum;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Question
{
    public class QuestionSettingUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown publishDropdown, gradeDropdown;

        private void Start()
        {
            publishDropdown.onValueChanged.AddListener((e)=>SetQuestionSetting());
            gradeDropdown.onValueChanged.AddListener((e)=>SetQuestionSetting());
        }


        public void SetQuestionSetting()
        {
            Grade grade = (Grade)gradeDropdown.value;
            Publisher publisher = (Publisher)publishDropdown.value;
            
            Debug.Log($"grade:{grade}, publish:{publisher}");

            var questionSetting = new QuestionSetting(grade, publisher);

            QuestionManager.Instance.SetQuestionSetting(questionSetting);
        }
    }
}