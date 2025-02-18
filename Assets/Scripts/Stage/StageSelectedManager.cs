using System.Collections.Generic;
using Characters.Ally;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Stage
{
    /// <summary>
    /// 關卡選擇器
    /// </summary>
    public class StageSelectedManager : SerializedMonoBehaviour
    {
        public static StageSelectedManager Instance => GameManager.Instance.stageSelectedManager;
        
        [Required]
        [SerializeField] private StageDataOverview _stageDataOverview;
        
        [LabelText("目前選擇的關卡")]
        [SerializeField]
        private StageName _currentStageData;
        
        [LabelText("目前選擇的玩家")]
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