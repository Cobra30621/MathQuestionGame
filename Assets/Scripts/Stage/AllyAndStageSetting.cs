using System.Collections.Generic;
using Characters.Ally;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stage
{
    public class AllyAndStageSetting : ScriptableObject
    {
        [LabelText("可選擇個關卡清單")]
        [SerializeField] private List<StageName> _stageDataList;
        
        [LabelText("可選擇的玩家清單")]
        [SerializeField] private List<AllyName> _allyNameList;

        public List<StageName> StageNameList => _stageDataList;
        
        public List<AllyName> AllyNameList => _allyNameList;
    }
}