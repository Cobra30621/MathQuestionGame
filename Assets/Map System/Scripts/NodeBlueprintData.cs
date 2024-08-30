using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    public class NodeBlueprintData : ScriptableObject
    {
        [TableList(ShowIndexLabels = true)]
        [InlineEditor()]
        public List<NodeBlueprint> NodeBlueprints;
        
        public NodeBlueprint GetNodeBlueprint(NodeType nodeType)
        {
            var nodeBlueprint = NodeBlueprints.Find(bp => bp.nodeType == nodeType);
            if (nodeBlueprint == null)
            {
                Debug.LogError($"找不到 NodeBlueprint 類別: {nodeType}");
                return null;
            }

            return nodeBlueprint;
        }
    }
}