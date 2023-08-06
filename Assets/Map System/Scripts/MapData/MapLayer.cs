using System.Collections.Generic;
using OneLine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class MapLayer
    {
        [Title("此層的節點清單")]
        [TableColumnWidth(200), VerticalGroup("每層的節點清單")]
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]

        public List<NodeData> nodeDatas;
        
        [VerticalGroup("距離設定")]
        [Title("與前一層的距離")]
        [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer;
        [VerticalGroup("距離設定")]
        [Title("此層中，節點間距離")]
        public float nodesApartDistance;
        [VerticalGroup("距離設定")]
        [Title("節點隨機偏移")]
        [Range(0f, 1f)] public float randomizePosition;
        [InfoBox("如果設置為 0，該層上的節點將顯示為一條直線。 越接近 1f = 更多位置隨機化")]

        public NodeData GetNodeData(int index)
        {
            if (index >= nodeDatas.Count)
            {
                Debug.LogError($"nodeDatas 的數量只有 {nodeDatas.Count}，無法取得 index: {index}");
                return null;
            }
            else
            {
                return nodeDatas[index];
            }

            
        }
    }
}