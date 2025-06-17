using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Question.UI
{
    public class QuestionCountToggle : MonoBehaviour
    {
        private Toggle _toggle;

        [ShowInInspector]
        private int _questionCount;

        [Required]
        [SerializeField] private TextMeshProUGUI info;

        private SelectedQuestionUI _selectedQuestionUI;
        
        private void Awake()
        {
            _toggle.onValueChanged.AddListener(OnToggleSelected);
        }

        public void Init(SelectedQuestionUI selectedQuestionUI, ToggleGroup toggleGroup,  int questionCount)
        {
            _selectedQuestionUI = selectedQuestionUI;
           _questionCount = questionCount;
           info.text = $"{questionCount}";
           
           _toggle = GetComponent<Toggle>();
           _toggle.group = toggleGroup;
        }

        private void OnToggleSelected(bool selected)
        {
            if(selected)
                _selectedQuestionUI.SetQuestionCount(_questionCount);
        }
    }
}