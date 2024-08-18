using System.Collections.Generic;
using Map_System.Scripts.MapData;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stage
{
    /// <summary>
    /// 不同難度關卡
    /// </summary>
    [CreateAssetMenu(fileName = "Stage Data", menuName = "Stage Data")]
    public class StageData : SODataBase<StageDataOverview>
    {
        public string stageName;
        
        [LabelText("每一層數據")]
        public List<MapConfig> maps = new List<MapConfig>();
        
    }
}