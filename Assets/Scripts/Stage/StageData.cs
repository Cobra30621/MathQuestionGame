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
        [TableColumnWidth(150 )]
        [LabelText("金錢掉落倍率")]
        public float moneyDropRate = 1f;
        
        [TableColumnWidth(700)]
        [LabelText("每一層數據")]
        public List<MapConfig> maps = new List<MapConfig>();
        
    }
}