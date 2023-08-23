using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Question
{
    /// <summary>
    /// 讀取本地端的數學題目(Demo 版用)
    /// </summary>
    public class LocalQuestionGenerator : IQuestionGenerator
    {
        private readonly CsvLoader _csvLoader = new CsvLoader();
        [InlineEditor()]
        [SerializeField] private LoadQuestionsData loadQuestionsData;

        private readonly int generateQuestionCount = 10;
        
        /// <summary>
        /// 取得數學題目清單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override List<Question> GetQuestions(QuestionSetting request)
        {
            // Get QuestionClip
            var loadQuestionsParameters = new List<LoadQuestionsParameter>();
            
            foreach (var publisher in request.Publishers)
            {
                foreach (var grade in request.Grades)
                {
                    var parameter = loadQuestionsData.GetParameter(publisher, grade);
                    loadQuestionsParameters.Add(parameter);
                }
            }


            if (loadQuestionsParameters.Count == 0)
            {
                Debug.LogError($"loadQuestionsParameters.Count = {loadQuestionsParameters.Count}");
            }
            // GetQuestion
            var questions = new List<Question>();
            foreach (var clip in loadQuestionsParameters)
            {
                questions.AddRange(GetQuestions(clip, generateQuestionCount));
            }

            return questions;
        }

        /// <summary>
        /// 取得單一 Question Clip 的數學問題
        /// </summary>
        /// <param name="loadQuestionsParameter"></param>
        /// <param name="generateCount"></param>
        /// <returns></returns>
        private List<Question> GetQuestions(LoadQuestionsParameter loadQuestionsParameter, int generateCount)
        {
            var questions = new List<Question>();

            string[][] datas = _csvLoader.LoadData(loadQuestionsParameter.QuestionCsv);
            // 去除標題
            datas = datas.Skip(1).ToArray();

            List<int> indexes = GenerateIndexes(generateCount, datas.Length);

            foreach (var index in indexes)
            {
                string[] row = datas[index];
                string questionName = row[0];
                string optionName = row[1];
                int answer = Convert.ToInt32(row[2]);

                // Debug.Log($"path {path}");
                Sprite questionSprite =  Resources.Load<Sprite> ( loadQuestionsParameter.FolderPath + questionName);
                Sprite optionSprite =  Resources.Load<Sprite> ( loadQuestionsParameter.FolderPath + optionName);

                Question question = new Question()
                {
                    Publisher = loadQuestionsParameter.Publisher,
                    Grade = loadQuestionsParameter.Grade,
                    
                    Answer = answer,
                    QuestionSprite = questionSprite,
                    OptionSprite = optionSprite
                };
                questions.Add(question);
            }

            // Debug.Log($"{loadQuestionsParameter}'s questions :{questions.Count}");

            return questions;
        }
        
        private List<int> GenerateIndexes(int generateCount, int dataCount)
        {
            List<int> indexes = new List<int>();
            if (generateCount > dataCount)
            {
                generateCount = dataCount;
            }
        
            Random random = new Random();
            HashSet<int> usedIndexes = new HashSet<int>();
        
            while (indexes.Count < generateCount)
            {
                int newIndex = random.Next(0, dataCount);
                if (!usedIndexes.Contains(newIndex))
                {
                    indexes.Add(newIndex);
                    usedIndexes.Add(newIndex);
                }
            }
        
            return indexes;
        }
    }
}