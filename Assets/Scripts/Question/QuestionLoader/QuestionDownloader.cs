using System.Collections;
using System.Collections.Generic;
using System.IO;
using Question.Enum;
using UnityEngine;
using UnityEngine.Networking;

namespace Question.QuestionLoader
{
    public class QuestionDownloader : MonoBehaviour
    {
        private const string get_question_api_url = "https://api.emath.math.ncu.edu.tw/problem/serial/";
        private const string get_image_api_url = "https://api.emath.math.ncu.edu.tw/problem/";
        private const string auth_token = "sRu564CCjtoGGIkd050b2YZulLGqjdrTT7mzGWJcJPM5rvE7RKr7Wpmff/Ah";
        
        private bool imageDownloadSuccess;
        private List<Data.Question> tempQuestions;

        public IEnumerator DownloadQuestion(Publisher publisher, Grade grade, int chapter, 
            int hard, bool saveLocal, List<Data.Question> questions)
        {
            string url = GetURL(publisher, grade, chapter, hard);

            var www = UnityWebRequest.Get(url);
            www.SetRequestHeader("Authorization", auth_token);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                var data = JsonUtility.FromJson<ProblemData>(json);

                Problem problem = data.problem[0];
                var question = new global::Question.Data.Question()
                {
                    Answer = int.Parse(problem.answer),
                    Publisher = publisher,
                    Grade = grade,
                    questionName = problem.problemLink,
                    optionsName = problem.ansLink
                };

                imageDownloadSuccess = true;
                yield return DownloadImageWithRetry(problem.problemLink, saveLocal, question, true );
                yield return DownloadImageWithRetry(problem.ansLink, saveLocal, question, false );


                // 圖片下載成功，才能放入問題集
                if (imageDownloadSuccess)
                {
                    questions.Add(question);
                }
                
                www.Dispose();
                Debug.Log($"[下載成功]（{publisher}, {grade}, Chapter: {chapter}, Hard: {hard}\n{url}");
            }
            else
            {
                Debug.LogError($"[下載失敗] 題目 API 連線失敗（{publisher}, {grade}, Chapter: {chapter}, " +
                               $"Hard: {hard}\n{url}");
            }
        }
        
        
        private string GetURL(Publisher publisher, Grade grade, int chapter,  int hard)
        {
            int justGrade = ((int)grade + 2) / 2;
            int semester = ((int)grade) % 2 + 1;
            int count = 1;

            return get_question_api_url + $"{(int)publisher + 1}{justGrade}{semester}{chapter:D2}{hard}001/{count}";
        }
        
        private IEnumerator DownloadImageWithRetry(string problemLink,  bool saveLocal, Data.Question question, bool isQuestionImage)
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
                // 存到本地端
                if (saveLocal)
                {
                    byte[] imageBytes = texture.EncodeToPNG();
                    string savePath = Path.Combine("Assets/Resources/Question", $"{problemLink}.png");
                    File.WriteAllBytes(savePath, imageBytes);
                }
                
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                
                if (isQuestionImage)
                    question.QuestionSprite = sprite;
                else
                    question.OptionSprite = sprite;
            }
            else
            {
                Debug.LogError($"[失敗] 圖片下載失敗: {imageUrl}");
                imageDownloadSuccess = false;
            }

            www?.Dispose();
        }
    }
}