using System.Collections;
using System.Collections.Generic;
using System.IO;
using Question.Enum;
using UnityEngine;
using UnityEngine.Networking;

namespace Question.QuestionLoader
{
    /// <summary>
    /// 負責從 API 下載題目與對應圖片，並處理圖片下載失敗的重試與本地儲存。
    /// </summary>
    public class QuestionDownloader : MonoBehaviour
    {
        private const string GetQuestionApiUrl = "https://api.emath.math.ncu.edu.tw/problem/serial/";
        private const string GetImageApiUrl = "https://api.emath.math.ncu.edu.tw/problem/";
        private const string AuthToken = "sRu564CCjtoGGIkd050b2YZulLGqjdrTT7mzGWJcJPM5rvE7RKr7Wpmff/Ah";

        private bool _imageDownloadSuccess;

        /// <summary>
        /// 從 API 下載一題題目並附帶圖片，成功後加入指定的問題集合。
        /// </summary>
        public IEnumerator DownloadQuestion(Publisher publisher, Grade grade, int chapter, int difficulty, bool saveLocal, List<Data.Question> questions)
        {
            string url = ComposeQuestionUrl(publisher, grade, chapter, difficulty);

            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", AuthToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                ProblemData data = JsonUtility.FromJson<ProblemData>(json);

                Problem problem = data.problem[0];
                var question = new global::Question.Data.Question
                {
                    Answer = int.Parse(problem.answer),
                    choiceCounts = int.Parse(problem.choices),
                    Publisher = publisher,
                    Grade = grade,
                    questionName = problem.problemLink,
                    optionsName = problem.ansLink
                };

                _imageDownloadSuccess = true;
                // 下載問題圖片
                yield return DownloadImageWithRetry(problem.problemLink, saveLocal, question, true);
                // 下載選項圖片
                yield return DownloadImageWithRetry(problem.ansLink, saveLocal, question, false);

                // 只有問題與選項都下載成功，才會存入清單中
                if (_imageDownloadSuccess)
                {
                    questions.Add(question);
                    Debug.Log($"[下載成功]（{publisher}, {grade}, Chapter: {chapter}, Hard: {difficulty}\n{url}");
                }else
                {
                    Debug.LogWarning($"[下載失敗] 圖片下載失敗（{publisher}, {grade}, Chapter: {chapter}, Hard: {difficulty}\n{url}");
                }
            }
            else
            {
                Debug.LogWarning($"[下載失敗] 題目 API 連線失敗（{publisher}, {grade}, Chapter: {chapter}, Hard: {difficulty}\n{url}");
                Debug.LogWarning($"[下載失敗] 題目 API 連線失敗（{publisher}, {grade}, Chapter: {chapter}, Hard: {difficulty}\n{url}");
            }

            request.Dispose();
        }

        /// <summary>
        /// 根據出版社、年級、章節與難度組成 API 所需的題目查詢 URL。
        /// </summary>
        private string ComposeQuestionUrl(Publisher publisher, Grade grade, int chapter, int difficulty)
        {
            int justGrade = ((int)grade + 2) / 2;
            int semester = ((int)grade) % 2 + 1;
            int count = 1;

            return GetQuestionApiUrl + $"{(int)publisher + 1}{justGrade}{semester}{chapter:D2}{difficulty}001/{count}";
        }

        /// <summary>
        /// 嘗試下載指定圖片（題目或選項圖片），最多重試三次，並可選擇是否儲存到本地。
        /// </summary>
        private IEnumerator DownloadImageWithRetry(string imageLink, bool saveLocal, Data.Question question, bool isQuestionImage)
        {
            string imageUrl = GetImageApiUrl + imageLink;
            int retryCount = 0;
            bool success = false;
            UnityWebRequest imageRequest = null;

            while (!success && retryCount < 3)
            {
                imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
                imageRequest.SetRequestHeader("Authorization", AuthToken);
                yield return imageRequest.SendWebRequest();

                if (imageRequest.result == UnityWebRequest.Result.Success)
                {
                    success = true;
                }
                else
                {
                    retryCount++;
                    Debug.LogWarning($"[重試第 {retryCount} 次] 圖片下載失敗: {imageUrl}\n錯誤: {imageRequest.error}");
                    yield return new WaitForSeconds(0.5f);
                }
            }

            if (success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
                if (saveLocal)
                {
                    byte[] imageBytes = texture.EncodeToPNG();
                    string savePath = Path.Combine("Assets/Resources/Question", imageLink + ".png");
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
                Debug.LogWarning($"[失敗] 圖片下載失敗: {imageUrl}");
                _imageDownloadSuccess = false;
            }

            imageRequest?.Dispose();
        }
    }
}
