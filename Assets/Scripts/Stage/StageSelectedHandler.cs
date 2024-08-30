using NueGames.Data.Characters;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Stage
{
    public class StageSelectedHandler : SerializedMonoBehaviour
    {
        [Required]
        [SerializeField] private StageDataOverview _stageDataOverview;
        
        private StageName _currentStageData;
        private AllyData _currentAllyData;
        
        
        public UnityEvent<AllyData> OnAllyDataChanged;
        
        public void SetStageData(StageName stageData)
        {
            _currentStageData = stageData;
            // Additional logic to handle the selected stage
        }

        public void SetAllyData(AllyData allyData)
        {
            _currentAllyData = allyData;
            OnAllyDataChanged.Invoke(_currentAllyData);
            // Additional logic to handle the selected ally
        }

        public StageName GetStageData()
        {
            return _currentStageData;
        }

        public AllyData GetAllyData()
        {
            return _currentAllyData;
        }

        public float GetMoneyDropRate()
        {
            return _stageDataOverview.FindUniqueId(_currentStageData.Id).moneyDropRate;
        }
    }
}