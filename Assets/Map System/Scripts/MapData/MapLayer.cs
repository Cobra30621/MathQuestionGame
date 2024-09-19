using System.Collections.Generic;
using NueGames.Encounter;
using OneLine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class MapLayer
    {
        [TableColumnWidth(200), VerticalGroup("每層的節點清單")]
        [GUIColor("#46A3FF")]
        [LabelText("此節點事件清單-機率設定")]
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
        [ValidateInput("@nodeTypeClips.Count > 0", "節點事件清單不能為空")]
        [SerializeField] public List<NodeTypeClip> nodeTypeClips;
        
        public int nodeCount => nodeCountRange.GetValue();

        [VerticalGroup("每層的節點清單")] 
        [LabelText("節點數量")]
        public IntMinMax nodeCountRange = new IntMinMax(){min = 2, max = 3};
        
        [VerticalGroup("距離設定")]
        [LabelText("與前一層的距離")]
        public float distanceFromPreviousLayer = 3f;
        
        [VerticalGroup("距離設定")]
        [LabelText("此層中，節點間距離")]
        public float nodesApartDistance = 2;
        
        [VerticalGroup("距離設定")]
        [LabelText("節點隨機偏移")]
        [Range(0f, 1f)] public float randomizePosition = 0.2f;
        [InfoBox("如果設置為 0，該層上的節點將顯示為一條直線。 越接近 1f = 更多位置隨機化")]

        public NodeType GetNodeTypeByWeight()
        {
            var typeClips = WeightedRandom.GetWeightedRandomObjects(nodeTypeClips, 1);

            if (typeClips.Count == 0)
            {
                Debug.LogError($"nodeTypeClips 為空的，請去設定");
                return NodeType.MinorEnemy;
            }
            
            return typeClips[0].NodeType;
        }
    }
}