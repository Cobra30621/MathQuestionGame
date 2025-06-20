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
using UnityEngine.Serialization;

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
        
        [Required]
        [SerializeField] private GameObject _noInternetWarningUI;
        
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
        /// 當網路不佳下載失敗時是否改用本地資料
        /// </summary>
        [LabelText("網路失敗時是否使用本地題庫")]
        [SerializeField] private bool _fallbackToLocalIfNoInternet = true;
 
        
        /// <summary>
        /// 開始答題的流程
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator StartQuestionFlow(QuestionActionBase action)
        {
            Session = new QuestionSession();
            _fallbackToLocalIfNoInternet = action.fallbackToLocalIfNoInternet;
            
            bool downloadSuccess = false;
            bool waitDownloadDone = false;

            // 呼叫下載並等待 callback 結果
            yield return StartCoroutine(DownloadOnlineQuestions(success =>
            {
                downloadSuccess = success;
                waitDownloadDone = true;
            }));

            // 等待 callback 結束（保險用）
            yield return new WaitUntil(() => waitDownloadDone);

            if (!downloadSuccess && !_fallbackToLocalIfNoInternet)
            {
                Debug.LogWarning("網路訊號不好，下載失敗，無法進行答題流程。");
                _noInternetWarningUI.SetActive(true);
                yield break;
            }

            // 初始化與後續流程
            InitializeSession(action);
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


        private IEnumerator DownloadOnlineQuestions(Action<bool> onComplete)
        {
            _waitDownloadUI.Show();

            int needQuestionCount = QuestionManager.Instance.QuestionSetting.needAnswerCount;

            bool downloadSuccess = false;

            yield return _generator.GenerateQuestion((bool success, List<Data.Question> questions) =>
            {
                downloadSuccess = success;
                Session.SetQuestions(questions);
                isOnlineText.text = success ? "使用線上題目" : "使用本地端題庫";
            }, needQuestionCount);

            _waitDownloadUI.Close();

            onComplete?.Invoke(downloadSuccess);
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