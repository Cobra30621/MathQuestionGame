using System.Collections.Generic;
using NueGames.Data.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stage
{
    public class AllyAndStageSetting : ScriptableObject
    {
        [LabelText("可選擇個關卡清單")]
        [SerializeField] private List<StageName> _stageDataList;
        
        [InlineEditor]
        [LabelText("可選擇的玩家清單")]
        [SerializeField] private List<AllyData> _allyDataList;

        public List<StageName> StageNameList => _stageDataList;
        
        public List<AllyData> AllyDataList => _allyDataList;
    }
}