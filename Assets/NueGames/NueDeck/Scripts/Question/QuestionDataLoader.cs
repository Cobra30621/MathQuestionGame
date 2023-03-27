using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Question
{
    public class QuestionDataLoader : MonoBehaviour
    {
        private readonly CsvLoader _csvLoader = new CsvLoader();
        private string inputPath = "Questions/";
        private string outputPath = "Assets/Data/Questions/";


        [ContextMenu("Test")]
        public void Test()
        {
            string chapterName = "angle";
            LoadQuestionData(chapterName);
        }


        private void LoadQuestionData(string chapterName)
        {
            QuestionsData questionsData = CreateQuestionData(chapterName);
            questionsData.MultipleChoiceQuestions = new List<MultipleChoiceQuestion>();
            
            string[][] datas = _csvLoader.LoadData(inputPath + chapterName);
            
            foreach (string[] data in datas)
            {
                int answer = Convert.ToInt32(data[0]) - 1;
                int optionCount = 4;
                
                string spriteName = data[1];
                Sprite questionSprite =  Resources.Load<Sprite> (inputPath + spriteName + "a");   
                Sprite optionSprite =  Resources.Load<Sprite> (inputPath + spriteName + "b");   
                
                MultipleChoiceQuestion question = new MultipleChoiceQuestion(answer, optionCount, questionSprite, optionSprite);
                questionsData.MultipleChoiceQuestions.Add(question);
            }
        }

        private QuestionsData CreateQuestionData(string chapterName)
        {
            QuestionsData questionsData = ScriptableObject.CreateInstance<QuestionsData>();
            AssetDatabase.CreateAsset(questionsData, outputPath + chapterName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = questionsData;
            questionsData.SetChapterName(chapterName);
            
            return questionsData;
        }
    }
}