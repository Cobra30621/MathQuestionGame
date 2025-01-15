using NueGames.Card;
using Question;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Log;
using Managers;
using NueGames.Enums;
using Question.Action;
using Question.Answer_Button;
using Question.Data;
using Question.QuestionGenerator;
using Question.QuestionLoader;
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
        
        /// <summary>
        /// 答題介面
        /// </summary>
        [Required]
        [SerializeField] private QuestionController questionController;

        /// <summary>
        /// 線上問題下載器
        /// </summary>
        [Required]
        [SerializeField] private OnlineQuestionDownloader onlineQuestionDownloader;
        
        /// <summary>
        /// 本地端題目產生器
        /// </summary>
        [Required]
        [SerializeField] private IQuestionGetter localQuestionGetter;
        
        /// <summary>
        /// 線上題目產生器
        /// </summary>
        [Required]
        [SerializeField] private IQuestionGetter onlineQuestionGetter;
        
        
        /// <summary>
        /// 答題按鈕
        /// </summary>
        [SerializeField] private AnswerButtonBase[] answerButtons;
        

        #region Public 變數
        /// <summary>
        /// 已經答題數量
        /// </summary>
        public int HasAnswerCount => answerRecord.AnswerCount;
        /// <summary>
        /// 答對數量
        /// </summary>
        public int CorrectAnswerCount => answerRecord.CorrectCount;
        /// <summary>
        /// 答錯數量
        /// </summary>
        public int WrongAnswerCount => answerRecord.WrongCount;
        

        #endregion

        #region 暫存 Cache
        
        
        /// <summary>
        /// 題目清單
        /// </summary>
        [SerializeField] private List<Data.Question> questionList;
        /// <summary>
        /// 正在回答的題目
        /// </summary>
        private Data.Question _currentQuestion;
        
        
        /// <summary>
        /// 這次進入答題介面的答題紀錄
        /// </summary>
        [SerializeField]  private AnswerRecord answerRecord;
        /// <summary>
        /// 這場戰鬥的答題記錄
        /// </summary>
        [SerializeField] private AnswerRecord combatAnswerRecord;

        
        #endregion
        
        
        /// <summary>
        /// 正在答題中
        /// </summary>
        public bool IsQuestioning => isQuestioning;
        /// <summary>
        /// 正在答題中
        /// </summary>
        [SerializeField] private bool isQuestioning;
        /// <summary>
        /// 等待玩家答題中
        /// </summary>
        [SerializeField] private bool waitAnswer;
        /// <summary>
        /// 等待動畫撥放
        /// </summary>
        [SerializeField] private bool waitPlayingAnimation;


        
        #region 事件

        /// <summary>
        /// 事件: 答題
        /// </summary>
        public System.Action OnAnswerQuestion;
        /// <summary>
        /// 事件: 答對題目
        /// </summary>
        public System.Action OnAnswerCorrect;
        /// <summary>
        /// 事件: 答錯題目
        /// </summary>
        public System.Action OnAnswerWrong;
        /// <summary>
        /// 事件: 結束答題模式
        /// </summary>
        public Action<int> OnQuestioningModeEnd;

        #endregion

        /// <summary>
        /// 數學行動
        /// </summary>
        
        private QuestionActionBase QuestionAction;
        /// <summary>
        /// 數學參數
        /// </summary>
        public QuestionSetting QuestionSetting;

        public int QuestionCount => QuestionAction.QuestionCount;
        
       

        void Awake()
        {
            isQuestioning = false;
        }



        #region 事件
        /// <summary>
        /// 戰鬥開始時的初始化
        /// </summary>
        public void OnCombatStart()
        {
            combatAnswerRecord.Clear();
        }
        
        /// <summary>
        /// 回答問題時
        /// </summary>
        /// <param name="option"></param>
        public void OnAnswer(int option)
        {
            EnableAnswer(false);
            OnAnswerQuestion?.Invoke();

            bool isCorrect = option == _currentQuestion.Answer;

            var outcomeStr = isCorrect ? "正確" : "錯誤";
            EventLogger.Instance.LogEvent(LogEventType.Question, $"答題 - {outcomeStr}", 
                $"選擇: {option}, 正確: {_currentQuestion.Answer}");
            
            if (isCorrect)
            {
                answerRecord.CorrectCount++;
                combatAnswerRecord.CorrectCount++;
                questionController.OnAnswer(true, option);
                OnAnswerCorrect?.Invoke();
            }
            else
            {
                answerRecord.WrongCount++;
                combatAnswerRecord.WrongCount++;
                questionController.OnAnswer(false, option);
                OnAnswerWrong?.Invoke();
            }

            answerRecord.AnswerCount++;
            combatAnswerRecord.AnswerCount++;
            
            waitAnswer = false;
            
        }

        

        #endregion
        

        #region Public Method

        public void StartDownloadOnlineQuestions()
        {
            onlineQuestionDownloader.DownloadQuestion(QuestionSetting.Publishers[0], QuestionSetting.Grades[0]);
        }
        
        /// <summary>
        /// 進入答題模式
        /// </summary>
        /// <param name="newParameters"></param>
        public void EnterQuestionMode(QuestionActionBase questionAction)
        {
            QuestionAction = questionAction;
            StartCoroutine(QuestionCoroutine());
        }
        

        public AnswerButtonBase GetAnswerButton(int option)
        {
            if (option-1 >= answerButtons.Length)
            {
                Debug.LogError($"{option} 超過 answerbuttons 數量");
                return null;
            }

            return answerButtons[option-1];
        }

        

        public int GetAnswerCountInThisCombat(AnswerOutcomeType answerOutcomeType)
        {
            switch (answerOutcomeType)
            {
                case AnswerOutcomeType.Answer:
                    return combatAnswerRecord.AnswerCount;
                case AnswerOutcomeType.Correct:
                    return combatAnswerRecord.CorrectCount;
                case AnswerOutcomeType.Wrong :
                    return combatAnswerRecord.WrongCount;
            }

            Debug.LogError($"未設定 {answerOutcomeType} 的 GetAnswerCountInThisCombat");
            return 0;
        }

        public void SetQuestionSetting(QuestionSetting setting)
        {
            QuestionSetting = setting;
            SaveManager.Instance.SavePermanentGame();
        }

        #endregion
        
        #region Questioning Coroutine
        
        IEnumerator QuestionCoroutine()
        {
            // Init
            answerRecord.Clear();
            isQuestioning = true;
            
            // 進入答題介面
            questionController.EnterQuestionMode();
            while (waitPlayingAnimation) yield return null; // 等待開頭反饋特效
           

            // Start Questioning
            while (isQuestioning)
            {
                NextQuestion();
                questionController.SetNextQuestion(_currentQuestion);
                while (waitPlayingAnimation) yield return null; // 等待顯示題目反饋特效
                
                EnableAnswer(true);
                waitAnswer = true;
                while (waitAnswer) yield return null;
                // yield return new WaitForSeconds(0.5f);
                
                while (waitPlayingAnimation) yield return null; // 等待答題成功反饋特效
                
                JudgeEndConditions();
            }
            
            // questionController.ExitQuestionMode("");
            while (waitPlayingAnimation) yield return null; // 等待離開動畫反饋特效
            yield return new WaitForSeconds(0.1f);
            
            OnQuestioningFinish();
            
            PlayAfterQuestioningAction();

        }


        public void StartPlayAnimation()
        {
            waitPlayingAnimation = true;
        }
        
        public void OnAnimationFinish()
        {
            waitPlayingAnimation = false;
        }

        public void OnQuestioningFinish()
        {
            questionController.ClosePanel();
        }
        
        
        void EnableAnswer(bool enable)
        {
            foreach (var answerButton in answerButtons)
            {
                answerButton.EnableAnswer(enable);
            }
        }

        void NextQuestion()
        {
            if (questionList.Count == 0)
            {
                GenerateQuestions();
            }
            
            int index = new System.Random().Next(questionList.Count);
            _currentQuestion = questionList[index];
            questionList.RemoveAt(index);
        }

        [Button("GenerateQuestions")]
        public void GenerateQuestions()
        {
            if (onlineQuestionGetter.EnableGetQuestion())
            {
                questionList = onlineQuestionGetter.GetQuestions(QuestionSetting);
                EventLogger.Instance.LogEvent(LogEventType.Question, $"使用線上題目 - 數量 {questionList.Count}");
            }
            else
            {
                questionList = localQuestionGetter.GetQuestions(QuestionSetting);
                EventLogger.Instance.LogEvent(LogEventType.Question, $"使用本地題目 - 數量 {questionList.Count}");
            }
        }
        
        private void PlayAfterQuestioningAction()
        {
            if (answerRecord.CorrectCount >= QuestionAction.NeedCorrectCount)
            {
                QuestionAction.DoCorrectAction();
            }
            else
            {
                QuestionAction.DoWrongAction();
            }
        }
        
  

        
        #endregion
        
        #region Judge End Questioning Mode Condition
        private void JudgeEndConditions()
        {
            if (answerRecord.AnswerCount >= QuestionAction.QuestionCount)
            {
                ExitQuestionMode("魔法詠唱結束");
            }
        }

        
        void ExitQuestionMode(string info)
        {
            isQuestioning = false;
            questionController.ExitQuestionMode(info);
        }
        
        #endregion

        
        public void LoadData(PermanentGameData data)
        {
            QuestionSetting = data.QuestionSetting;
        }

        public void SaveData(PermanentGameData data)
        {
            data.QuestionSetting = QuestionSetting;
        }
    }

    /// <summary>
    /// 答題紀錄
    /// </summary>
    [Serializable]
    public class AnswerRecord
    {
        public int AnswerCount;
        public int CorrectCount;
        public int WrongCount;


        public void Clear()
        {
            AnswerCount = 0;
            CorrectCount = 0;
            WrongCount = 0;
        }
    }
}
