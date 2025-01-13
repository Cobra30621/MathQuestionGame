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
            Debug.Log(logMessage.GetDetailMessage()); // 在 Console 顯示
        }

        
        public List<string> GetEventLog()
        {
            return _logMessages.ConvertAll(x => x.GetMessage());
        }
        
        public List<string> GetDetailEventLog()
        {
            return _logMessages.ConvertAll(x => x.GetDetailMessage());
        }
        
    }

    [Serializable]
    public class LogEvent
    {
        private LogEventType LogEventType;
        public string Title;
        public string Detail;

        private DateTime time;

        
        public LogEvent(LogEventType logEventType, string title, string detail)
        {
            LogEventType = logEventType;
            Title = title;
            Detail = detail;

            time = DateTime.Now;
        }


        public string GetMessage()
        {
            return $"【{LogEventType}】{Title}  - {time:HH:mm:ss}";
        }
        
        public string GetDetailMessage()
        {
            return $"{GetMessage()}" +
                   $"\n{Detail}";
        }
    }

    [Serializable]
    public enum LogEventType
    {
        Save,
        Economy,
        MapEncounter,
        MapEvent,
        Combat,
        Main,
        Relic,
        Card,
        Scene,
        Question,
        Other
    }
}