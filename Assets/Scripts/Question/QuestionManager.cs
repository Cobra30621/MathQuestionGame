using NueGames.Card;
using Question;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Log;
using Managers;
using NueGames.Enums;
using Question.Action;
using Question.Answer_Button;
using Question.Core;
using Question.Data;
using Question.QuestionLoader;
using Question.UI;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Question
{
    /// <summary>
    /// 答題模式管理器
    /// </summary>
    public class QuestionManager : MonoBehaviour, IPermanentDataPersistence
    {
        public static QuestionManager Instance => GameManager.Instance.QuestionManager;

        [Required] [SerializeField] private QuestionGenerator _generator;
        
        [Required] [SerializeField] private QuestionUIController uiController;
        
        [Required] [SerializeField] private QuestionDisplay questionDisplay;

        [Required] [SerializeField] private QuestionOutcomeUI outcomeUI;
        
        private QuestionFlowController _flowController;


        private QuestionSession _session;


        public QuestionActionBase QuestionAction;

        /// <summary>
        /// 答題參數設定
        /// </summary>
        public QuestionSetting QuestionSetting;

        #region 初始化

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _session = new QuestionSession(_generator);
            _flowController = new QuestionFlowController(_session, uiController);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 開始下載線上題目
        /// </summary>
        public void StartDownloadOnlineQuestions()
        {
            _generator.StartDownloadOnlineQuestions(QuestionSetting);
        }

        /// <summary>
        /// 進入答題模式
        /// </summary>
        public void EnterQuestionMode(QuestionActionBase action)
        {
            QuestionAction = action;
            StartCoroutine(_flowController.StartQuestionFlow(action));
        }

        /// <summary>
        /// 答題
        /// </summary>
        /// <param name="option"></param>
        public void Answer(int option)
        {
            _flowController.HandleAnswer(option);
        }

        /// <summary>
        /// 設置動畫播放完畢(給 animator 使用)
        /// </summary>
        public void OnAnimationFinish()
        {
            questionDisplay.OnAnimationFinish();
        }

        /// <summary>
        /// 顯示答題結果
        /// </summary>
        public void ShowOutcome()
        {
            outcomeUI.ShowOutcome(_session.AnswerRecord);
        }

        #endregion


        #region 答題設定

        public void SetQuestionSetting(QuestionSetting setting)
        {
            QuestionSetting = setting;
            SaveManager.Instance.SavePermanentGame();
        }

        public void LoadData(PermanentGameData data)
        {
            QuestionSetting = data.QuestionSetting;
        }

        public void SaveData(PermanentGameData data)
        {
            data.QuestionSetting = QuestionSetting;
        }

        #endregion
    }
}