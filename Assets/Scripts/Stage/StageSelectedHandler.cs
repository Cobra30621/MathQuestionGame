using System.Collections.Generic;
using Characters.Ally;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Stage
{
    public class StageSelectedHandler : SerializedMonoBehaviour
    {
        [Required]
        [SerializeField] private StageDataOverview _stageDataOverview;
        
        [SerializeField]
        private StageName _currentStageData;
        [SerializeField]
        private AllyData _currentAllyData;
        
        
        public UnityEvent<AllyData> OnAllyDataChanged;
        
        public void SetStageData(StageName stageData)
        {
            _currentStageData = stageData;
        }

        public void SetAllyData(AllyData allyData)
        {
            _currentAllyData = allyData;
            OnAllyDataChanged.Invoke(_currentAllyData);
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