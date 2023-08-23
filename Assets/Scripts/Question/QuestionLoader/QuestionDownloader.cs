using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class QuestionDownloader : MonoBehaviour
{
    public int generateCount;
    
    private const string first_api_url = "https://api.emath.math.ncu.edu.tw/problem/serial/111011002/";
    
    private const string api_url = "https://api.emath.math.ncu.edu.tw/problem/";
    private const string auth_token = "sRu564CCjtoGGIkd050b2YZulLGqjdrTT7mzGWJcJPM5rvE7RKr7Wpmff/Ah";
    
    [FolderPath(RequireExistingPath = true)]
    [SerializeField] private string saveFolder;
    private string csv_filename = "api_results.csv";

    public ProblemData data;
    

    [Button]
    public void GetQuestion()
    {
        StartCoroutine(GetQuestionCoroutine());
    }

    private IEnumerator GetQuestionCoroutine()
    {
        using UnityWebRequest www = UnityWebRequest.Get(first_api_url + generateCount);
        www.SetRequestHeader("Authorization", $"{auth_token}");

        using (StreamWriter writer = new StreamWriter(Path.Combine(saveFolder, csv_filename), false, Encoding.UTF8))
        {
            writer.WriteLine("problemLink,ansLink,answer");
        }
        
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            Debug.Log($"response {json}");
            data = JsonUtility.FromJson<ProblemData>(json);
            Debug.Log($"data {data}");

            using (StreamWriter writer = new StreamWriter(Path.Combine(saveFolder, csv_filename), true, Encoding.UTF8))
            {
                for (int i = 0; i < generateCount; i++)
                {
                    Problem problem = data.problem[i];
                    writer.WriteLine($"{problem.problemLink},{problem.ansLink},{problem.answer}");
                }
            }
        }
        else
        {
            Debug.Log($"API request failed. Error: {www.error}");
        }

        
        Debug.Log("CSV file saved: " + csv_filename);
    }

    [Button]
    public void DownLoadImage()
    {
        StartCoroutine(ProcessCsv());
    }

    private IEnumerator ProcessCsv()
    {
        string csvFilePath = Path.Combine(saveFolder, csv_filename);

        if (File.Exists(csvFilePath))
        {
            string[] csvLines = File.ReadAllLines(csvFilePath);

            foreach (string csvLine in csvLines.Skip(1)) // Skip header row
            {
                string[] values = csvLine.Split(',');

                if (values.Length >= 3)
                {
                    string problemLink = values[0];
                    yield return DownloadImage(problemLink);
                    
                    string answerLink = values[1];
                    yield return DownloadImage(answerLink);
                }
            }
        }
        else
        {
            Debug.LogError($"CSV file not found: {csvFilePath}");
        }
    }

    private IEnumerator DownloadImage(string problemLink)
    {
        string imageUrl = $"{api_url}{problemLink}";
        string savePath = Path.Combine(saveFolder, $"{problemLink}.png");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        www.SetRequestHeader("Authorization", $"{auth_token}");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            byte[] imageBytes = texture.EncodeToPNG();

            File.WriteAllBytes(savePath, imageBytes);
            Debug.Log($"Image downloaded and saved: {savePath}");
        }
        else
        {
            Debug.LogError($"Image download failed. Error: {www.error}");
        }

        www.Dispose();
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
