using NueGames.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Question
{
    public class QuestionSettingUI : SerializedMonoBehaviour
    {
        [SerializeField] private TMP_Dropdown publishDropdown, gradeDropdown;


        public void SetQuestionSetting()
        {
            Grade grade = (Grade)gradeDropdown.value;
            Publisher publisher = (Publisher)publishDropdown.value;
            
            Debug.Log($"grade:{grade}, publish:{publisher}");

            var questionSetting = new QuestionSetting(grade, publisher);

            GameManager.Instance.SetQuestionSetting(questionSetting);
        }
    }
}