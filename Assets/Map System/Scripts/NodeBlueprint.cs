using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    public enum NodeType
    {
        [LabelText("小怪")]
        MinorEnemy = 0,
        [LabelText("精英怪")]
        EliteEnemy = 1,
        [LabelText("王")]
        Boss = 2,
        [LabelText("營火")]
        CampFire = 11,
        [LabelText("寶箱")]
        Treasure = 12,
        [LabelText("商店")]
        Store = 13,
        [LabelText("事件")]
        Event = 14,
    }
}

namespace Map
{
    
    
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        [TableColumnWidth(57, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public Sprite sprite;
        public NodeType nodeType;
    }
}