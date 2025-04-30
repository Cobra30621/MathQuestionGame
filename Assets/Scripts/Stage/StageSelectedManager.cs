using System.Collections.Generic;
using Characters.Ally;
using Managers;
using NueGames.Enums;
using Relic.Data;
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
        private AllyName currentAllyName;
        
        
        public UnityEvent<AllyData> OnAllyDataChanged;
        
        [Required]
        public AllyDataOverview allyDataOverview;
        
        public void SetStageData(StageName stageData)
        {
            _currentStageData = stageData;
        }

        public void SetAllyData(AllyName allyName)
        {
            currentAllyName = allyName;
            var allyData = GetAllyData();
            OnAllyDataChanged.Invoke(allyData);
        }

        public StageName GetStageData()
        {
            return _currentStageData;
        }

        public AllyName GetCurrentAllyName()
        {
            return currentAllyName;
        }
        
        public AllyData GetAllyData()
        {
            var allyData = allyDataOverview.FindUniqueId(currentAllyName.Id);
            return allyData;
        }

        public float GetMoneyDropRate()
        {
            return _stageDataOverview.FindUniqueId(_currentStageData.Id).moneyDropRate;
        }
        

        public List<RelicName> RewardDropRelic()
        {
            return GetAllyData().rewardDropRelic;
        }
    }
}