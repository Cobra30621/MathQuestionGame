using System.Collections;
using System.Collections.Generic;
using System.IO;
using Question.Data;
using Question.Enum;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Question.QuestionLoader
{
    /// <summary>
    /// 本地題庫下載器：從指定出版商與年級中，下載所有章節與難度的題目，並存入 QuestionData。
    /// </summary>
    public class LocalQuestionDownloader : MonoBehaviour
    {
        /// <summary>
        /// 儲存所有下載結果的資料容器。
        /// </summary>
        [InlineEditor]
        public QuestionData QuestionData;


        /// <summary>
        /// 每個章節每個難度要下載的題目數量。
        /// </summary>
        public int generateCountForEachChapter = 1;

        /// <summary>
        /// 實際執行下載的工具。
        /// </summary>
        [Required]
        public QuestionDownloader downloader;

        /// <summary>
        /// Unity 按鈕：開始下載所有題目。
        /// </summary>
        [Button("1. 取得題目")]
        public void GetQuestion()
        {
            StartCoroutine(GetAllQuestionsCoroutine());
        }

        /// <summary>
        /// 下載所有年級的題目，僅針對固定的 Publisher（目前為 Ziyou）。
        /// </summary>
        private IEnumerator GetAllQuestionsCoroutine()
        {
            QuestionData.questionClip = new List<QuestionClip>();

            foreach (Grade grade in System.Enum.GetValues(typeof(Grade)))
            {
                yield return GetQuestionsByGradeCoroutine(Publisher.Ziyou, grade);
            }
        }

        /// <summary>
        /// 針對特定的出版社與年級，下載所有章節與難度的題目。
        /// </summary>
        /// <param name="publisher">出版社</param>
        /// <param name="grade">年級</param>
        private IEnumerator GetQuestionsByGradeCoroutine(Publisher publisher, Grade grade)
        {
            var questionClip = new QuestionClip
            {
                publisher = publisher,
                grade = grade,
                questions = new List<Data.Question>()
            };

            // 章節 (1 ~ 12)
            for (int chapter = 1; chapter <= 12; chapter++)
            {
                // 難度 (1 ~ 8)
                for (int difficulty = 1; difficulty <= 8; difficulty++)
                {
                    for (int i = 0; i < generateCountForEachChapter; i++)
                    {
                        yield return downloader.DownloadQuestion(publisher, grade, chapter, difficulty, true, questionClip.questions);
                    }
                }
            }

            Debug.Log($"✅ 成功下載 {publisher}, {grade}: 共 {questionClip.questions.Count} 題");
            QuestionData.questionClip.Add(questionClip);
        }

        /// <summary>
        /// 讀取 Resources 資料夾中對應的圖片資源，並指派到每一題中。
        /// </summary>
        [InfoBox("由於下載圖片需要時間，請確定圖片下載完成後，再去執行 2.按鈕")]
        [Button("2.將下載的題目，讀取到 QuestionData 中")]
        private void LoadAllSpriteFromResource()
        {
            foreach (var clip in QuestionData.questionClip)
            {
                foreach (var question in clip.questions)
                {
                    // 讀取題目圖片
                    Sprite questionSprite = Resources.Load<Sprite>($"Question/{question.questionName}");
                    Debug.Log($"讀取題目圖片: {questionSprite}");
                    question.QuestionSprite = questionSprite;

                    // 讀取選項圖片
                    Sprite optionSprite = Resources.Load<Sprite>($"Question/{question.optionsName}");
                    question.OptionSprite = optionSprite;
                }
            }

#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
    }
}
