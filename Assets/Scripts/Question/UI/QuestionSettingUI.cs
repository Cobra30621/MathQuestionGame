using System;
using Question.Data;
using Question.Enum;
using TMPro;
using UnityEngine;

namespace Question.UI
{
    public class QuestionSettingUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown publishDropdown, gradeDropdown;

        private void Start()
        {
            publishDropdown.onValueChanged.AddListener((e)=>SetQuestionSetting());
            gradeDropdown.onValueChanged.AddListener((e)=>SetQuestionSetting());

            QuestionManager.onQuestionSettingChange.AddListener(UpdateDropdown);
        }

        private void OnEnable()
        {
            UpdateDropdown(QuestionManager.Instance.QuestionSetting);
        }


        private void UpdateDropdown(QuestionSetting questionSetting)
        {
            var publish = (int)questionSetting.Publisher;
            publishDropdown?.SetValueWithoutNotify(publish);
            
            var grade = (int)questionSetting.Grade;
            gradeDropdown?.SetValueWithoutNotify(grade);
        }


        public void SetQuestionSetting()
        {
            Grade grade = (Grade)gradeDropdown.value;
            Publisher publisher = (Publisher)publishDropdown.value;
            

            var questionSetting = new QuestionSetting(grade, publisher);

            QuestionManager.Instance.SetQuestionSetting(questionSetting);
        }
    }
}