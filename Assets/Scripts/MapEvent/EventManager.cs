using Encounter;
using Log;
using Managers;
using MapEvent.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace MapEvent
{
    /// <summary>
    /// 处理事件的执行和选项的管理
    /// </summary>
    [RequireComponent(typeof(SceneChanger))]
    public class EventManager : MonoBehaviour
    {
        [LabelText("事件清單")]
        [InlineEditor]
        public EventList eventList;

        [LabelText("離開選項")]
        public OptionData leaveOption;
        
        public static UnityEvent OnLeaveEventSystem = new UnityEvent();
        public static UnityEvent<Option> OnExecuteCompleted = new UnityEvent<Option>();


        public static EventManager Instance  => GameManager.Instance != null ? GameManager.Instance.EventManager : null;
        
        
        private SceneChanger _sceneChanger;
        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
        }
        
        
        /// <summary>
        /// 取得離開的選項
        /// </summary>
        /// <returns></returns>
        public Option GetLeaveOption()
        {
            return EventFactory.GetOption(leaveOption);
        }
        
        
        /// <summary>
        /// 从事件列表中获取随机事件
        /// </summary>
        [Button]
        public Event GetRandomEvent()
        {
            if (eventList.Events == null || eventList.Events.Count == 0)
            {
                Debug.LogError("事件列表为空");
                return null;
            }
            int randomIndex = Random.Range(0, eventList.Events.Count);
            var data = eventList.Events[randomIndex];
            
            EventLogger.Instance.LogEvent(LogEventType.MapEvent, $"開啟事件: {data.Text}", 
                $"{data.Description}");
            return EventFactory.GetEvent(data);
        }

        /// <summary>
        /// 处理选项执行完成的回调
        /// </summary>
        public static void OnOptionExecuteCompleted(Option option)
        {
            EventLogger.Instance.LogEvent(LogEventType.MapEvent, $"完成事件選項: {option.data.OptionText}");
            // 在这里添加选项执行完成后的逻辑
            OnExecuteCompleted.Invoke(option);
        }

        public void LeaveEventSystem()
        {
            OnLeaveEventSystem.Invoke();
            EncounterManager.Instance.OnRoomCompleted();
            
            StartCoroutine(_sceneChanger.OpenMapScene());
        }
        
        
    }
}