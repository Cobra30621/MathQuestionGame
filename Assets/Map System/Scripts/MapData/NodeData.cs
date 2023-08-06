using System;
using System.Collections.Generic;
using NueGames.Encounter;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class NodeData
    {
        [SerializeField] private List<NodeTypeClip> nodeTypeClips;
        
        /// <summary>
        /// 依照權重，取的節點事件
        /// </summary>
        /// <returns></returns>
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
    
    [Serializable]
    public class NodeTypeClip : IWeightedObject{
        
        [SerializeField] private NodeType nodeType;
        [SerializeField] private int weight = 1;
        public NodeType NodeType => nodeType;
        public int Weight => weight;
    }
}