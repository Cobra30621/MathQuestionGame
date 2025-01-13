using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Log
{
    /// <summary>
    /// 紀錄玩家的重要事件資訊
    /// </summary>
    public class EventLogger : MonoBehaviour
    {
        public static EventLogger Instance => GameManager.Instance.EventLogger;
        
        [SerializeField] private List<LogEvent> _logMessages;
        
        public void LogEvent(LogEventType logEventType, string eventName, string details = "")
        {
            var logMessage = new LogEvent(logEventType, eventName, details);
            _logMessages.Add(logMessage);
            Debug.Log(logMessage.GetMessage()); // 在 Console 顯示
        }

        public List<string> GetEventLog()
        {
            return _logMessages.ConvertAll(x => x.GetMessage());
        }
        
    }

    [Serializable]
    public class LogEvent
    {
        private LogEventType LogEventType;
        public string Title;
        public string Detail;

        
        public LogEvent(LogEventType logEventType, string title, string detail)
        {
            LogEventType = logEventType;
            Title = title;
            Detail = detail;
        }
        
        public string GetMessage()
        {
            return $"【{LogEventType}】{Title}" +
                   $"\n{Detail}";
        }
    }

    public enum LogEventType
    {
        Save,
        Economy,
        MapEncounter,
        Combat,
        Main,
        Relic,
        Card,
        Scene,
        Question,
        Other
    }
}