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

            var publish = (int)QuestionManager.Instance.QuestionSetting.Publishers[0];
            publishDropdown?.SetValueWithoutNotify(publish);
            
            var grade = (int)QuestionManager.Instance.QuestionSetting.Grades[0];
            gradeDropdown?.SetValueWithoutNotify(grade);
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