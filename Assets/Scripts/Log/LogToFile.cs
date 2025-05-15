using System;
using System.IO;
using Sentry;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Log
{
    public class LogToFile : MonoBehaviour
    {
        // void OnEnable()
        // {
        //     // 訂閱所有 log 訊息
        //     Application.logMessageReceived += HandleLog;
        // }
        //
        // void OnDisable()
        // {
        //     Application.logMessageReceived -= HandleLog;
        // }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            // 只對 Error 或 Exception 類型觸發
            if (type == LogType.Error || type == LogType.Exception)
            {
                // 執行事件：將錯誤訊息與堆疊傳出去
                SentrySdk.CaptureMessage($"{logString}\n" +
                                         $"{stackTrace}:\n" +
                                         $"Simple: {EventLogger.Instance.GetEventLog()}");
                SentrySdk.CaptureMessage($"{stackTrace}:\n" +
                                         $"{stackTrace}:\n" +
                                         $"Detail: {EventLogger.Instance.GetDetailEventLog()}");
            }
        }
        
        
        [Button]
        public static void SaveLogToFile()
        { 
            var logFilePath = Path.Combine(Application.persistentDataPath, "user_logs.txt");
            var detailLogFilePath = Path.Combine(Application.persistentDataPath, "user_detail_logs.txt");
            
            File.WriteAllLines(logFilePath, EventLogger.Instance.GetEventLog());
            File.WriteAllLines(detailLogFilePath, EventLogger.Instance.GetDetailEventLog());
            Debug.Log($"Log saved to {logFilePath}");
            Debug.Log($"Log saved to {detailLogFilePath}");

            
        }

        private void OnApplicationQuit()
        {
            SaveLogToFile();
        }
    }
}