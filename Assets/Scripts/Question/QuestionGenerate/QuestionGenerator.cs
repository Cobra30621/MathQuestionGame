using System.Collections.Generic;
using Log;
using Question.Data;
using Question.QuestionGenerate;
using Question.QuestionLoader;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question
{
    /// <summary>
    /// 負責生成題目
    /// </summary>
    public class QuestionGenerator : MonoBehaviour
    {
        [Required]
        [SerializeField] private IQuestionGetter _localGetter;
        [Required]
        [SerializeField] private IQuestionGetter _onlineGetter;
        /// <summary>
        /// 線上問題下載器
        /// </summary>
        [Required]
        [SerializeField] private OnlineQuestionDownloader onlineQuestionDownloader;


        /// <summary>
        /// 下載線上題庫
        /// </summary>
        /// <param name="setting"></param>
        public void StartDownloadOnlineQuestions(QuestionSetting setting)
        {
            onlineQuestionDownloader.DownloadQuestion(setting.Publishers[0], setting.Grades[0]);
        }
        
        /// <summary>
        /// 取得題目清單
        /// </summary>
        /// <returns></returns>
        public List<Data.Question> GenerateQuestions()
        {
            var setting = QuestionManager.Instance.QuestionSetting;
            
            if (_onlineGetter.EnableGetQuestion(setting))
            {
                var questions = _onlineGetter.GetQuestions(setting);
                EventLogger.Instance.LogEvent(LogEventType.Question, $"使用線上題目 - 數量 {questions.Count}");
                return questions;
            }
            
            var localQuestions = _localGetter.GetQuestions(setting);
            EventLogger.Instance.LogEvent(LogEventType.Question, $"使用本地題目 - 數量 {localQuestions.Count}");
            return localQuestions;
        }
    }
}