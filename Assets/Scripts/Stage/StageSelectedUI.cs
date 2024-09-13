using System.Collections.Generic;
using Managers;
using NueGames.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stage
{
    public class StageSelectedUI : MonoBehaviour
    {
        private int _currentStageIndex;
        private List<StageName> _stageNameList;

        [Required]
        [SerializeField]
        private TextMeshProUGUI currentStageNameText;

        [Required]
        [SerializeField]
        private Button previousButton;

        [Required]
        [SerializeField]
        private Button nextButton;

        [Required]
        [SerializeField] private StageDataOverview _stageDataOverview;

        private void Start()
        {
            previousButton.onClick.AddListener(SetPrevious);
            nextButton.onClick.AddListener(SetNext);
        }

        public void Init(List<StageName> stageDataList)
        {
            _stageNameList = stageDataList;
            _currentStageIndex = 0;

            OnStageSelected();
            UpdateUI();
        }

        public void SetPrevious()
        {
            if (_currentStageIndex > 0)
            {
                _currentStageIndex--;
                OnStageSelected();
                UpdateUI();
            }
        }
        
        public void SetNext()
        {
            if (_currentStageIndex < _stageNameList.Count - 1)
            {
                _currentStageIndex++;
                OnStageSelected();
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            var stageName = _stageNameList[_currentStageIndex];
            var stageData = _stageDataOverview.FindUniqueId(stageName.Id);
            currentStageNameText.text = stageData.DisplayName;
        }

        private void OnStageSelected()
        {
            StageName currentStageName = _stageNameList[_currentStageIndex];
            
            var selectedHandler = GameManager.Instance.StageSelectedHandler;
            selectedHandler.SetStageData(currentStageName);
        }
    }
}