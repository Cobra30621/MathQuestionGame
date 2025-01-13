using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Log
{
    public class LogToFile : MonoBehaviour
    {
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