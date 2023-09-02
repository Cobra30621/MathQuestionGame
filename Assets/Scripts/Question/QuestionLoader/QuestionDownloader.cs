using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Question;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class QuestionDownloader : MonoBehaviour
{
   
    [InlineEditor()]
    public QuestionData QuestionData;
    
    public int generateCountForEachChapter;
    public int hard;
    public int maxChapter;
    
    // API 相關的 URL 和授權令牌
    private const string get_question_api_url = "https://api.emath.math.ncu.edu.tw/problem/serial/";
    private const string get_image_api_url = "https://api.emath.math.ncu.edu.tw/problem/";
    private const string auth_token = "sRu564CCjtoGGIkd050b2YZulLGqjdrTT7mzGWJcJPM5rvE7RKr7Wpmff/Ah";
    

    public ProblemData data;

    [Button("取得題目")]
    public void GetQuestion()
    {
        StartCoroutine(GetAllQuestionCoroutine());
    }
    
    /// <summary>
    /// 協程，用於下載所有問題
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetAllQuestionCoroutine()
    {
        QuestionData.questionClip = new List<QuestionClip>();
        // foreach (Publisher publisher in Enum.GetValues(typeof(Publisher)))
        // {
        //     foreach (Grade grade in Enum.GetValues(typeof(Grade)))
        //     {
        //         yield return GetQuestionCoroutine(publisher, grade);
        //        
        //     }
        // }
        
        foreach (Grade grade in Enum.GetValues(typeof(Grade)))
        {
            yield return GetQuestionCoroutine(Publisher.Ziyou, grade);
               
        }
    }
    
    /// <summary>
    /// 協程，用於下載指定出版商和年級的問題
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="grade"></param>
    /// <returns></returns>
    private IEnumerator GetQuestionCoroutine(Publisher publisher, Grade grade)
    {
        var questionClip = new QuestionClip()
        {
            publisher = publisher,
            grade = grade,
            questions = new List<Question.Question>()
        };
        
        for (int chapter = 1; chapter < maxChapter + 1; chapter++)
        {
            string url = GetURL(publisher, grade, chapter, generateCountForEachChapter);
        
            using UnityWebRequest www = UnityWebRequest.Get(url + generateCountForEachChapter);
            www.SetRequestHeader("Authorization", $"{auth_token}");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                data = JsonUtility.FromJson<ProblemData>(json);
                
                for (int i = 0; i < generateCountForEachChapter; i++)
                {
                    Problem problem = data.problem[i];
                    var question = new Question.Question()
                    {
                        Answer = int.Parse(problem.answer),
                        Publisher = publisher,
                        Grade = grade,
                        questionName = problem.problemLink,
                        optionsName =  problem.ansLink
                    };
                    yield return DownloadImage(problem.problemLink);
                    yield return DownloadImage(problem.ansLink);
                
                    questionClip.questions.Add(question);
                }
                
                Debug.Log($"<color=green>API request success {url}</color>");
            }
            else
            {
                Debug.LogError($"API request failed. {url}\nError: {www.error}");
            }
        }
        QuestionData.questionClip.Add(questionClip);
    }

    /// <summary>
    /// 組合 API 請求的 URL
    /// </summary>
    /// <param name="publisher"></param>
    /// <param name="grade"></param>
    /// <param name="chapter"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private string GetURL(Publisher publisher, Grade grade, int chapter, int count)
    {
        int justGrade = ((int)grade+2) / 2;
        int semester = ((int)grade) % 2 + 1;
        
        return get_question_api_url + $"{(int)publisher + 1}{justGrade}{semester}{chapter:D2}{hard}001/{count}";
    }
    
    /// <summary>
    ///  協程，用於下載圖片
    /// </summary>
    /// <param name="problemLink"></param>
    /// <returns></returns>
    private IEnumerator DownloadImage(string problemLink)
    {
        string imageUrl = $"{get_image_api_url}{problemLink}";
        string savePath = Path.Combine("Assets/Resources/Question", $"{problemLink}.png");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        www.SetRequestHeader("Authorization", $"{auth_token}");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            byte[] imageBytes = texture.EncodeToPNG();
            File.WriteAllBytes(savePath, imageBytes);
            
            // Debug.Log($"Image downloaded and saved: {savePath}");
        }
        else
        {
            Debug.LogError($"Image download failed. Error: {www.error}");
        }

        www.Dispose();

        // yield return new WaitForSeconds(0.1f);
    }


    [InfoBox("由於下載圖片需要時間，請確定圖片下載完成後，再去執行 2.按鈕")]
    [Button("2.將下載的題目，讀取到 QuestionData中")]
    private void LoadAllSpriteFromResource()
    {
        foreach (var clip in QuestionData.questionClip)
        {
            foreach (var question in clip.questions)
            {
                Sprite questionSprite = Resources.Load<Sprite>($"Question/{question.questionName}");
                question.QuestionSprite = questionSprite;
                
                Sprite answerSprite = Resources.Load<Sprite>($"Question/{question.optionsName}");
                question.OptionSprite = answerSprite;
            }
        }
        
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
