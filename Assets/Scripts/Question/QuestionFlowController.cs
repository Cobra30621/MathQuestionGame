using System;
using System.Collections;
using System.Collections.Generic;
using Log;
using Newtonsoft.Json;
using Question.Action;
using Question.QuestionGenerate;
using Question.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Question.Core
{
    /// <summary>
    /// 控制答題流程
    /// </summary>
    [Serializable]
    public class QuestionFlowController : MonoBehaviour
    {
        /// <summary>
        /// 管理單次答題階段的狀態和記錄
        /// </summary>
        public QuestionSession Session { private set; get; }
        
        
        /// <summary>
        /// 負責控制答題相關的 UI 
        /// </summary>
        [Required]
        [SerializeField] 
        private QuestionUIController _uiController;

        [Required]
        [SerializeField] private WaitDownloadUI _waitDownloadUI;
        
        /// <summary>
        /// 問題產生器
        /// </summary>
        [Required]
        [SerializeField]  private QuestionGenerator _generator;

        [Required] [SerializeField] private TextMeshProUGUI isOnlineText;
        
        /// <summary>
        /// 正在答題中
        /// </summary>
        [ShowInInspector]
        private bool _isQuestioning;
        
        /// <summary>
        /// 等待玩家回答問題
        /// </summary>
        [ShowInInspector]
        private bool _waitingForAnswer;
        
 
        
        /// <summary>
        /// 開始答題的流程
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator StartQuestionFlow(QuestionActionBase action)
        {
            Session = new QuestionSession();
            // 下載題目
            yield return DownloadOnlineQuestions();
            
            InitializeSession(action);
            
            // 答題迴圈
            yield return RunQuestioningLoop(action);
            
            FinishSession(action);
        }

        
        /// <summary>
        /// 初始化答題紀錄
        /// </summary>
        /// <param name="action"></param>
        private void InitializeSession(QuestionActionBase action)
        {
            Session.StartNewSession(action);
            _isQuestioning = true;
            _uiController.UpdateAnswerRecord(Session);
        }


        private IEnumerator DownloadOnlineQuestions()
        {
            _waitDownloadUI.Show();

            int needQuestionCount = QuestionManager.Instance.QuestionSetting.needAnswerCount;
            // 下載題目
            yield return _generator.GenerateQuestion((bool isOnline, List<Data.Question> questions) =>
            {
                Session.SetQuestions(questions);
                isOnlineText.text = isOnline ? "使用線上題目" : "使用本地端題庫";
            }, needQuestionCount);
            
            _waitDownloadUI.Close();
        }
        
        
        private IEnumerator RunQuestioningLoop(QuestionActionBase action)
        {
            // 開始時封鎖答題按鈕
            _uiController.SetEnableAnswer(false);
            // 進入答題介面
            yield return _uiController.EnterQuestionMode();
            
            // 主要答題循環
            while (_isQuestioning)
            {
                // 顯示下一個題目
                yield return ShowNextQuestion();
                // 等待玩家答題
                yield return WaitForAnswer();
                // 檢查是否結束
                CheckEndCondition();
            }
            
            // 離開答題介面
            yield return _uiController.ExitQuestionMode("數學答題結束");
        }

        
        /// <summary>
        /// 設定下一個題目的流程
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowNextQuestion()
        {
            // 取得題目
            var nextQuestion = Session.GetNextQuestion();
            // 等待 UI 動畫包放完畢
            yield return _uiController.ShowNextQuestion(nextQuestion);
            // 等待玩家答題
            _waitingForAnswer = true;
            // 動畫播放完後，設置可以回答問題
            _uiController.SetEnableAnswer(true);
        }
        
        /// <summary>
        /// 等待玩家答題
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForAnswer()
        {
            // 等待玩家回答問題
            yield return new WaitUntil(()=> !_waitingForAnswer);
        }
        
        /// <summary>
        /// 檢查並設定是否答題結束
        /// </summary>
        private void CheckEndCondition()
        {
            if (Session.IsFinishAllQuestion())
            {
                _isQuestioning = false;
            }
        }

        
        /// <summary>
        /// 回答問題
        /// </summary>
        /// <param name="option"></param>
        public void HandleAnswer(int option)
        {
            bool isCorrect = option == Session.CurrentQuestion.Answer;
            Session.RecordAnswer(isCorrect);
            
            var outcomeStr = isCorrect ? "正確" : "錯誤";
            EventLogger.Instance.LogEvent(LogEventType.Question, $"答題 - {outcomeStr}", 
                $"選擇: {option}, 正確: { Session.CurrentQuestion.Answer}");
            
            _uiController.SetEnableAnswer(false);
            _uiController.HandleAnswerFeedback(isCorrect, option);
            _uiController.UpdateAnswerRecord(Session);
            _waitingForAnswer = false;
        }
        
        
        /// <summary>
        /// 完成本次答題
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void FinishSession(QuestionActionBase action)
        {
            // 執行是答題數量達到條件
            if (Session.ReachSuccessCondition())
            {
                action.DoCorrectAction();
            }
            else
            {
                action.DoWrongAction();
            }
        }
    }
}