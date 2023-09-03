using Sirenix.OdinInspector;
using UnityEngine;

namespace Map
{
    public enum NodeType
    {
        MinorEnemy = 0,
        EliteEnemy = 1,
        Boss = 2,
        CampFire = 11,
        Treasure,
        Store,
        Mystery,
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