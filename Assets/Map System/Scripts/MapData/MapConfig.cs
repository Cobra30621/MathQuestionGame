using System.Collections.Generic;
using Malee;
using NueGames.Data.Encounter;
using OneLine;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// 一層地圖的資料
    /// </summary>
    [CreateAssetMenu(fileName = "MapConfig", menuName = "NueDeck/Map/MapConfig")]
    public class MapConfig : ScriptableObject
    {
        public List<NodeBlueprint> nodeBlueprints;
        public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

        [OneLineWithHeader]
        public IntMinMax numOfPreBossNodes;
        [OneLineWithHeader]
        public IntMinMax numOfStartingNodes;

        [Tooltip("Increase this number to generate more paths")]
        public int extraPaths;
        [Reorderable]
        public ListOfMapLayers layers;

        /// <summary>
        /// 遭遇事件
        /// </summary>
        public EncounterStage encounterStage;

        [System.Serializable]
        public class ListOfMapLayers : ReorderableArray<MapLayer>
        {
        }
    }
}