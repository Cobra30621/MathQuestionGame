using System.Collections;
using System.Collections.Generic;
using System.IO;
using Question.Data;
using Question.Enum;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace Question.QuestionLoader
{
    public class LocalQuestionDownloader : MonoBehaviour
    {
   
        [InlineEditor()]
        public QuestionData QuestionData;
        
        /// <summary>
        /// 最大章節數
        /// </summary>
        public const int maxChapter = 12;

        /// <summary>
        /// 最大難度
        /// </summary>
        public const int maxHard = 8;

        /// <summary>
        /// 每一章節，要下載的題數目
        /// </summary>
        public int generateCountForEachChapter = 1;

        [Required]
        public QuestionDownloader downloader;


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
        
            foreach (Grade grade in System.Enum.GetValues(typeof(Grade)))
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
                questions = new List<Data.Question>()
            };
            
            // 下載所有章節
            for (int chapter = 1; chapter < maxChapter + 1; chapter++)
            {
                // 下載所有難度
                for (int hard = 1; hard < maxHard + 1; hard++)
                {
                    for (int i = 0; i < generateCountForEachChapter; i++)
                    {
                        yield return downloader.DownloadQuestion(publisher, grade, chapter,  hard, true, questionClip.questions);
                    }
                }
            }
            
            Debug.Log($"成功下載 {publisher}, {grade}: {questionClip.questions.Count} 個題目");
            
            QuestionData.questionClip.Add(questionClip);
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
                    Debug.Log($"questionSprite {questionSprite}");
                    question.QuestionSprite = questionSprite;

                    Sprite answerSprite = Resources.Load<Sprite>($"Question/{question.optionsName}");
                    question.OptionSprite = answerSprite;
                }
            }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
    }
}

