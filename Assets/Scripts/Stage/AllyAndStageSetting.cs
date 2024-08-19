using System.Collections.Generic;
using NueGames.Data.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stage
{
    public class AllyAndStageSetting : ScriptableObject
    {
        [SerializeField] private List<StageName> _stageDataList;
        [InlineEditor]
        [SerializeField] private List<AllyData> _allyDataList;

        public List<StageName> StageNameList => _stageDataList;
        
        public List<AllyData> AllyDataList => _allyDataList;
    }
}