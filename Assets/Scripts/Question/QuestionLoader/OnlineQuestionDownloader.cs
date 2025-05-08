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
    public class OnlineQuestionDownloader : MonoBehaviour
    {
        public List<Data.Question> questions;

   
        private bool imageDownloadSueecss;
        
        [Required]
        public QuestionDownloader downloader;

        public IEnumerator DownloadQuestionsCoroutine(Publisher publisher, Grade grade, int totalQuestionCount)
        {
            EventLogger.Instance.LogEvent(LogEventType.Question, "下載 - 線上題目",
                $"出版社 {publisher}, 年級 {grade}");

            questions = new List<Data.Question>();

            while (questions.Count < totalQuestionCount)
            {
                // 隨機選一個單元下載
                int chapter = Random.Range(0, 12) + 1;
                int hard = Random.Range(0, 8) + 1;
                yield return downloader.DownloadQuestion(publisher, grade, chapter, hard, false, questions);
            }
            
            EventLogger.Instance.LogEvent(LogEventType.Question, $"下載完成 - 成功下載 {questions.Count} 題",
                $"出版社 {publisher}, 年級 {grade}");
        }


    }
}
