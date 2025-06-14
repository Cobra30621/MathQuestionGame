using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Question.Enum;
using Sirenix.OdinInspector;
using Log;
using System.IO;

namespace Question.QuestionLoader
{
    /// <summary>
    /// 負責從線上伺服器下載指定數量的題目資料，並儲存於 questions 列表中。
    /// </summary>
    public class OnlineQuestionDownloader : MonoBehaviour
    {
        /// <summary>
        /// 儲存下載後的題目列表。
        /// </summary>
        public List<Data.Question> questions;

        /// <summary>
        /// 問題下載器，負責與後端 API 溝通並取得題目。
        /// </summary>
        [Required]
        public QuestionDownloader downloader;

        /// <summary>
        /// 協同程序，用於依據指定的出版社與年級，從線上隨機下載指定數量的題目。
        /// 若網路下載失敗可結合 fallback 策略（尚未實作）。
        /// </summary>
        /// <param name="publisher">出版社列舉值。</param>
        /// <param name="grade">年級列舉值。</param>
        /// <param name="totalQuestionCount">要下載的題目總數。</param>
        /// <returns>IEnumerator，用於 Unity 協同程序。</returns>
        public IEnumerator DownloadQuestionsCoroutine(Publisher publisher, Grade grade, int totalQuestionCount)
        {
            // 紀錄開始下載的事件
            EventLogger.Instance.LogEvent(LogEventType.Question, "下載 - 線上題目",
                $"出版社: {publisher}, 年級: {grade}");

            // 初始化題目列表
            questions = new List<Data.Question>();
            
            while (questions.Count < totalQuestionCount)
            {
                // 隨機選擇一個章節 (1 ~ 8)
                //  (最大 12 單元，但大多只到第 8 單元)
                int chapter = Random.Range(1, 9);

                int difficulty;
                float roll = Random.value; // 會產生一個 0.0 ~ 1.0 的浮點數
                if (roll < 0.7f)
                {
                    // 70% 機率選 2 (2的題型最多)
                    difficulty =  2 ;
                }
                else
                {
                    // 30% 機率從 1~8 隨機
                    difficulty = Random.Range(1, 9);
                }

                // 啟動下載協程，將題目加入列表
                yield return downloader.DownloadQuestion(publisher, grade, chapter, difficulty, false, questions);
            }


            // 紀錄完成下載的事件
            EventLogger.Instance.LogEvent(LogEventType.Question, $"下載完成 - 成功下載 {questions.Count} 題",
                $"出版社: {publisher}, 年級: {grade}");
        }
    }
}