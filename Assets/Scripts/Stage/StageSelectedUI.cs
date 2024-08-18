using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    public class StageSelectedUI : MonoBehaviour
    {
        private int _currentStageIndex;
        private List<StageName> _stageDataList;

        public void Init(List<StageName> stageDataList)
        {
            _stageDataList = stageDataList;
            _currentStageIndex = 0;
            UpdateUI();
        }

        public void SetPrevious()
        {
            if (_currentStageIndex > 0)
            {
                _currentStageIndex--;
                UpdateUI();
            }
        }

        public void SetNext()
        {
            if (_currentStageIndex < _stageDataList.Count - 1)
            {
                _currentStageIndex++;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            // Update the UI elements with the data of the current stage
            StageName currentStageData = _stageDataList[_currentStageIndex];
            // Add your code to update the UI elements with the stage data
        }
    }
}