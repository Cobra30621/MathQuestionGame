using System.Collections.Generic;
using OneLine;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class MapLayer
    {
        public List<NodeData> nodeDatas;
        [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer;
        [Tooltip("Distance between the nodes on this layer")]
        public float nodesApartDistance;
        [Tooltip("If this is set to 0, nodes on this layer will appear in a straight line. Closer to 1f = more position randomization")]
        [Range(0f, 1f)] public float randomizePosition;


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