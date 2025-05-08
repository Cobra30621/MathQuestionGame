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

        private const string get_question_api_url = "https://api.emath.math.ncu.edu.tw/problem/serial/";
        private const string get_image_api_url = "https://api.emath.math.ncu.edu.tw/problem/";
        private const string auth_token = "sRu564CCjtoGGIkd050b2YZulLGqjdrTT7mzGWJcJPM5rvE7RKr7Wpmff/Ah";

        public ProblemData data;
        
        private List<Data.Question> tempQuestions;
        private bool imageDownloadSueecss;

        public IEnumerator DownloadQuestionsCoroutine(Publisher publisher, Grade grade, int totalQuestionCount, int hard)
        {
            EventLogger.Instance.LogEvent(LogEventType.Question, "下載 - 線上題目",
                $"出版社 {publisher}, 年級 {grade}");

            tempQuestions = new List<Data.Question>();

            while (tempQuestions.Count < totalQuestionCount)
            {
                // 隨機選一個單元下載
                int chapter = Random.Range(0, 12) + 1;
                int questionHard = Random.Range(0, 8) + 1;
                yield return DownloadQuestion(publisher, grade, chapter, questionHard);
            }
            
            questions = tempQuestions;
            EventLogger.Instance.LogEvent(LogEventType.Question, $"下載完成 - 成功下載 {tempQuestions.Count} 題",
                $"出版社 {publisher}, 年級 {grade}");
        }

        private IEnumerator DownloadQuestion(Publisher publisher, Grade grade, int chapter, int hard)
        {
            string url = GetURL(publisher, grade, chapter, 1, hard);
            bool success = false;
            var www = UnityWebRequest.Get(url);
            www.SetRequestHeader("Authorization", auth_token);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                data = JsonUtility.FromJson<ProblemData>(json);

                Problem problem = data.problem[0];
                var question = new global::Question.Data.Question()
                {
                    Answer = int.Parse(problem.answer),
                    Publisher = publisher,
                    Grade = grade,
                    questionName = problem.problemLink,
                    optionsName = problem.ansLink
                };

                imageDownloadSueecss = true;
                yield return DownloadImageWithRetry(question, problem.problemLink, true);
                yield return DownloadImageWithRetry(question, problem.ansLink, false);

                // 圖片下載成功，才能放入問題集
                if (imageDownloadSueecss)
                {
                    tempQuestions.Add(question);
                }
                
                www.Dispose();
                Debug.Log($"[成功] 下載題目 （Chapter: {chapter}, Hard: {hard}）: {url}");
            }
            else
            {
                Debug.LogError($"[失敗] 題目 API 連線失敗（Chapter: {chapter}, Hard: {hard}）: {url}");
            }
        }

        private string GetURL(Publisher publisher, Grade grade, int chapter, int count, int hard)
        {
            int justGrade = ((int)grade + 2) / 2;
            int semester = ((int)grade) % 2 + 1;

            return get_question_api_url + $"{(int)publisher + 1}{justGrade}{semester}{chapter:D2}{hard}001/{count}";
        }

        private IEnumerator DownloadImageWithRetry(Data.Question question, string problemLink, bool isQuestionImage)
        {
            string imageUrl = $"{get_image_api_url}{problemLink}";

            int retryCount = 0;
            bool success = false;
            UnityWebRequest www = null;

            while (!success && retryCount < 3)
            {
                www = UnityWebRequestTexture.GetTexture(imageUrl);
                www.SetRequestHeader("Authorization", auth_token);
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    success = true;
                }
                else
                {
                    retryCount++;
                    Debug.LogWarning($"[重試第 {retryCount} 次] 圖片下載失敗: {imageUrl}\n錯誤: {www.error}");
                    yield return new WaitForSeconds(0.5f);
                }
            }

            if (success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                if (isQuestionImage)
                    question.QuestionSprite = sprite;
                else
                    question.OptionSprite = sprite;
            }
            else
            {
                imageDownloadSueecss = false;
                Debug.LogError($"[失敗] 圖片下載失敗: {imageUrl}");
            }

            www?.Dispose();
        }

        [System.Serializable]
        public class ProblemData
        {
            public Problem[] problem;
        }

        [System.Serializable]
        public class Problem
        {
            public string problemLink;
            public string ansLink;
            public string answer;
        }
    }
}
