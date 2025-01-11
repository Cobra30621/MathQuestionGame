using System.Collections.Generic;
using MapEvent.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MapEvent
{
    /// <summary>
    /// 包含事件集合的可脚本化对象
    /// </summary>
    [CreateAssetMenu(fileName = "New Event List", menuName = "Event/Event List")]
    public class EventList : SerializedScriptableObject
    {
        [TableList(AlwaysExpanded = true)]
        public List<EventData> Events;


    }
}