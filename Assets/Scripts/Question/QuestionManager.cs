using NueGames.Card;
using Question;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Log;
using Managers;
using NueGames.Enums;
using Question.Action;
using Question.Core;
using Question.Data;
using Question.QuestionLoader;
using Question.UI;
using Save;
using Save.Data;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Question
{
    /// <summary>
    /// 答題模式管理器
    /// </summary>
    public class QuestionManager : MonoBehaviour, IPermanentDataPersistence
    {
        public static QuestionManager Instance => GameManager.Instance != null ? GameManager.Instance.QuestionManager : null;

        /// <summary>
        /// 流程控制器
        /// </summary>
        [SerializeField]
        [Required]
        private QuestionFlowController _flowController;
        
        #region UI
        /// <summary>
        /// 選擇答題數據介面
        /// </summary>
        [Required] [SerializeField] private SelectedQuestionUI selectedQuestionUI;
        
        /// <summary>
        /// 主要答題介面的 UI 控制
        /// </summary>
        [Required] [SerializeField] private QuestionUIController uiController;
        
        /// <summary>
        /// 主要答題介面的 UI 顯示
        /// </summary>
        [Required] [SerializeField] private QuestionDisplay questionDisplay;

        /// <summary>
        /// 答題結果介面
        /// </summary>
        [Required] [SerializeField] private QuestionOutcomeUI outcomeUI;
        
        
        #endregion
        
        public QuestionActionBase QuestionAction;

        /// <summary>
        /// 答題參數設定
        /// </summary>
        public QuestionSetting QuestionSetting;


        public static UnityEvent<QuestionSetting> onQuestionSettingChange = new UnityEvent<QuestionSetting>();

        #region Public Method

        /// <summary>
        /// 進入選擇題目介面
        /// </summary>
        public void EnterSelectedQuestionUI()
        {
            selectedQuestionUI.OpenPanel();
        }
        
        
        /// <summary>
        /// 進入答題模式
        /// </summary>
        public void EnterQuestionMode(QuestionActionBase action, int needAnswerCount)
        {
            QuestionAction = action;
            QuestionSetting.needAnswerCount = needAnswerCount;
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
            outcomeUI.ShowOutcome(_flowController.Session.AnswerRecord);
        }

        #endregion


        #region 答題設定

        public void SetQuestionSetting(QuestionSetting setting)
        {
            QuestionSetting = setting;
            onQuestionSettingChange.Invoke(setting);
            
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