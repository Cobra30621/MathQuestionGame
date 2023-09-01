using System.Collections.Generic;
using Malee;
using NueGames.Data.Encounter;
using OneLine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// 一層地圖的資料
    /// </summary>
    [CreateAssetMenu(fileName = "MapConfig", menuName = "NueDeck/Map/MapConfig")]
    public class MapConfig : SerializedScriptableObject
    {
        [Title("地圖層數設計")]
        [TableList(ShowIndexLabels = true)]
        public List<MapLayer> layers;
        /// <summary>
        /// 遭遇事件
        /// </summary>
        [Title("地圖遭遇事件")]
        public EncounterStage encounterStage;
        
        [TableList(ShowIndexLabels = true)]
        [InlineEditor()]
        public List<NodeBlueprint> nodeBlueprints;
        public int GridWidth => 2;
        // public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

        // [OneLineWithHeader]
        // public IntMinMax numOfPreBossNodes;
        // [OneLineWithHeader]
        // public IntMinMax numOfStartingNodes;

        // [Tooltip("Increase this number to generate more paths")]
        // public int extraPaths;
    }
}