using System;
using UnityEditor;
using UnityEngine;

namespace Question
{
    public class QuestionDataLoader : MonoBehaviour
    {
        private readonly CsvLoader _csvLoader = new CsvLoader();
        private string inputPath = "Questions/";
        private string outputPath = "Assets/Data/Questions/";
        [SerializeField] private string chapterName = "angle";

        
        
        [ContextMenu("Test")]
        public void Test()
        {
            LoadQuestionData(chapterName);
        }


        private void LoadQuestionData(string chapterName)
        {
            // 載入 Question Data
            string assetPath = outputPath + chapterName  + ".asset";
            QuestionsData questionsData = AssetDatabase.LoadAssetAtPath(assetPath, typeof(QuestionsData)) as QuestionsData;

            if (questionsData == null)
            {
                questionsData = CreateQuestionData(chapterName, assetPath);
                Debug.Log($"創造新的 QuestionData{chapterName}");
            }

            string[][] datas = _csvLoader.LoadData(inputPath + chapterName);
            foreach (string[] data in datas)
            {
                int answer = Convert.ToInt32(data[0]) - 1;
                int optionCount = 4;
                
                string spriteName = data[1];
                Sprite questionSprite =  Resources.Load<Sprite> (inputPath + spriteName + "a");   
                Sprite optionSprite =  Resources.Load<Sprite> (inputPath + spriteName + "b");   
                
                MultipleChoiceQuestion question = new MultipleChoiceQuestion(answer, optionCount, questionSprite, optionSprite);
                questionsData.AddQuestion(question);
            }
            
            
            // 宣告物件已經被修改，需要重新存檔。
            EditorUtility.SetDirty(questionsData);
            AssetDatabase.SaveAssets();
        }

        private QuestionsData CreateQuestionData(string chapterName, string assetPath)
        {
            QuestionsData questionsData = ScriptableObject.CreateInstance<QuestionsData>();
            AssetDatabase.CreateAsset(questionsData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = questionsData;
            questionsData.SetChapterName(chapterName);
            
            return questionsData;
        }
    }
}