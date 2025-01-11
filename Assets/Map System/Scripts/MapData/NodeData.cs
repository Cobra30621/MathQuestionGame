using System;
using System.Collections.Generic;
using Encounter.Data.EncounterList;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class NodeData
    {
        [GUIColor(0.3f, 0.8f, 0.8f)]
        [VerticalGroup("單一節點事件-機率設定")]
        [LabelText("此節點事件清單-機率設定")]
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
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
        
        [ VerticalGroup("事件類型")]
        [SerializeField] private NodeType nodeType;
        [ VerticalGroup("出現機率")]
        [SerializeField] private int weight = 1;
        public NodeType NodeType => nodeType;
        public int Weight => weight;
    }
}