using System;
using System.Collections;
using System.Collections.Generic;
using Log;
using Question.Data;
using Question.QuestionLoader;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question.QuestionGenerate
{
    /// <summary>
    /// 問答模式中的題目產生器：會優先下載線上題目，若失敗則使用本地資料
    /// </summary>
    public class QuestionGenerator : MonoBehaviour
    {
        [Required]
        [SerializeField] private LocalQuestionGetter _localGetter;

        [Required]
        [SerializeField] private OnlineQuestionDownloader _onlineDownloader;

        /// <summary>
        /// 每題最多等待下載的秒數（總等待時間 = 單題等待時間 × 題目數量）
        /// </summary>
        public float perQuestionMaxWaitTime = 1f;

        /// <summary>
        /// 啟動題目產生流程
        /// </summary>
        /// <param name="onQuestionReady">當題目準備好後的回呼 (成功, 題目列表)</param>
        /// <param name="needQuestionCount">所需題目數量</param>
        public IEnumerator GenerateQuestion(Action<bool, List<Data.Question>> onQuestionReady, int needQuestionCount)
        {
            var setting = QuestionManager.Instance.QuestionSetting;

            if (!IsInternetAvailable())
            {
                var local = LoadLocalQuestions(setting, onQuestionReady);
                yield break;
            }

            float timeout = perQuestionMaxWaitTime * needQuestionCount;

            // 嘗試下載題目，限制時間內完成
            yield return StartCoroutine(DownloadOnlineQuestionsWithTimeout(setting, needQuestionCount, timeout));

            List<Data.Question> onlineQuestions = _onlineDownloader.questions;

            if (onlineQuestions != null && onlineQuestions.Count >= needQuestionCount)
            {
                EventLogger.Instance.LogEvent(LogEventType.Question, $"使用線上題目 - 數量 {onlineQuestions.Count}");
                onQuestionReady.Invoke(true, onlineQuestions);
            }
            else
            {
                var local = LoadLocalQuestions(setting, onQuestionReady);
            }
        }

        /// <summary>
        /// 嘗試在限定時間內下載線上題目
        /// </summary>
        private IEnumerator DownloadOnlineQuestionsWithTimeout(QuestionSetting setting, int count, float timeout)
        {
            bool finished = false;

            StartCoroutine(DownloadQuestions(setting, count, () => finished = true));

            float elapsed = 0f;
            while (!finished && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// 啟動線上下載協程
        /// </summary>
        private IEnumerator DownloadQuestions(QuestionSetting setting, int count, System.Action onComplete)
        {
            yield return _onlineDownloader.DownloadQuestionsCoroutine(
                setting.Publisher,
                setting.Grade,
                count
            );
            onComplete?.Invoke();
        }

        /// <summary>
        /// 使用本地題目並回傳
        /// </summary>
        private List<Data.Question> LoadLocalQuestions(QuestionSetting setting, Action<bool, List<Data.Question>> callback)
        {
            var localQuestions = _localGetter.GetQuestions(setting);
            EventLogger.Instance.LogEvent(LogEventType.Question, $"使用本地題目 - 數量 {localQuestions.Count}");
            callback.Invoke(false, localQuestions);
            return localQuestions;
        }

        /// <summary>
        /// 是否連上網路（WiFi 或行動數據）
        /// </summary>
        public static bool IsInternetAvailable()
        {
            // Debug.Log($"Internet {Application.internetReachability}");
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}
