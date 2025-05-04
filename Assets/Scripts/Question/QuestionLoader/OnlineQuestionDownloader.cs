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

        public int maxChapter = 1;


        public IEnumerator DownloadQuestionCoroutine(Publisher publisher, Grade grade, int totalQuestionCount, int hard)
        {
            EventLogger.Instance.LogEvent(LogEventType.Question, "下載 - 線上題目",
                $"出版社 {publisher}, 年級 {grade}");

            var tempQuestions = new List<Data.Question>();
            int downloadedCount = 0;

            int chapterCount = maxChapter;
            int baseCountPerChapter = totalQuestionCount / chapterCount;
            int extra = totalQuestionCount % chapterCount;

            for (int chapter = 1; chapter <= chapterCount; chapter++)
            {
                if (downloadedCount >= totalQuestionCount)
                    break;

                int questionCountThisChapter = baseCountPerChapter + (chapter <= extra ? 1 : 0);

                string url = GetURL(publisher, grade, chapter, questionCountThisChapter, hard);

                UnityWebRequest www = null;
                bool success = false;
                int retryCount = 0;

                while (!success && retryCount < 3)
                {
                    www = UnityWebRequest.Get(url);
                    www.SetRequestHeader("Authorization", auth_token);
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        success = true;
                    }
                    else
                    {
                        retryCount++;
                        Debug.LogWarning($"[重試第 {retryCount} 次] 題目 API 下載失敗: {url}\n錯誤: {www.error}");
                        yield return new WaitForSeconds(1f);
                    }
                }

                if (!success)
                {
                    Debug.LogError($"[失敗] 題目 API 連線失敗（Chapter: {chapter}）: {url}");
                    continue;
                }

                string json = www.downloadHandler.text;
                data = JsonUtility.FromJson<ProblemData>(json);

                int availableCount = Mathf.Min(questionCountThisChapter, data.problem.Length);

                for (int i = 0; i < availableCount && downloadedCount < totalQuestionCount; i++)
                {
                    Problem problem = data.problem[i];
                    var question = new global::Question.Data.Question()
                    {
                        Answer = int.Parse(problem.answer),
                        Publisher = publisher,
                        Grade = grade,
                        questionName = problem.problemLink,
                        optionsName = problem.ansLink
                    };

                    yield return DownloadImageWithRetry(question, problem.problemLink, true);
                    yield return DownloadImageWithRetry(question, problem.ansLink, false);

                    tempQuestions.Add(question);
                    downloadedCount++;
                }

                www.Dispose();
            }

            questions = tempQuestions;
            EventLogger.Instance.LogEvent(LogEventType.Question, $"下載完成 - 成功下載 {tempQuestions.Count} 題",
                $"出版社 {publisher}, 年級 {grade}");
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
                    yield return new WaitForSeconds(1f);
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
