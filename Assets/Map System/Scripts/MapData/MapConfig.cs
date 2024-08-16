using System;
using System.Collections.Generic;
using Data.Encounter;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map_System.Scripts.MapData
{
    /// <summary>
    /// 一層地圖的資料
    /// </summary>
    [Serializable]
    public class MapConfig 
    {
        public string mapName;
        [LabelText("地圖層數設計")]
        [TableList(ShowIndexLabels = true)]
        public List<MapLayer> layers;
        
        [LabelText("地圖遭遇事件")]
        public EncounterStage encounterStage;

        public int GridWidth => 2;

  

    }
}