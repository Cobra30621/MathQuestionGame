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
        public IEnumerator GenerateQuestion(System.Action<bool, List<Data.Question>> onQuestionReady, int needQuestionCount)
        {
            var setting = QuestionManager.Instance.QuestionSetting;
            float totalWaitTime = perQuestionMaxWaitTime * needQuestionCount;

            List<Data.Question> onlineQuestions = new List<Data.Question>();
            bool downloadFinished = false;

            // 啟動線上題目下載協程，下載完成後設定旗標
            IEnumerator DownloadQuestions()
            {
                yield return _onlineDownloader.DownloadQuestionCoroutine(
                    setting.Publishers[0],
                    setting.Grades[0],
                    needQuestionCount,
                    1
                );

                onlineQuestions = _onlineDownloader.questions;
                downloadFinished = true;
            }

            // 開始執行下載協程
            StartCoroutine(DownloadQuestions());

            float elapsedTime = 0f;

            // 等待下載完成或超時
            while (!downloadFinished && elapsedTime < totalWaitTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 判斷是否成功下載足夠的題目
            if (onlineQuestions.Count >= needQuestionCount)
            {
                EventLogger.Instance.LogEvent(LogEventType.Question, $"使用線上題目 - 數量 {onlineQuestions.Count}");
                onQuestionReady.Invoke(true, onlineQuestions);
            }
            else
            {
                var localQuestions = _localGetter.GetQuestions(setting);
                EventLogger.Instance.LogEvent(LogEventType.Question, $"使用本地題目 - 數量 {localQuestions.Count}");
                onQuestionReady.Invoke(false, localQuestions);
            }
        }
    }
}
