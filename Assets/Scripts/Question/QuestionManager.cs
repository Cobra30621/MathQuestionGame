using NueGames.Card;
using Question;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using NueGames.Action.MathAction;
using NueGames.Enums;
using NueGames.Managers;

namespace Question
{
    /// <summary>
    /// 答題模式管理器
    /// </summary>
    public class QuestionManager : MonoBehaviour
    {
        private QuestionManager(){}
        public static QuestionManager Instance { get; private set; }
        
        /// <summary>
        /// 答題介面
        /// </summary>
        [SerializeField] private QuestionController questionController;
        /// <summary>
        /// 答題資料
        /// </summary>
        [SerializeField] private QuestionsData questionsData;
        /// <summary>
        /// 答題按鈕
        /// </summary>
        [SerializeField] private AnswerButtonBase[] answerButtons;

        /// <summary>
        /// 卡牌管理器
        /// </summary>
        private CollectionManager CollectionManager => CollectionManager.Instance;
        /// <summary>
        /// 事件管理器
        /// </summary>
        private EventManager EventManager => EventManager.Instance;


        #region Public 變數
        /// <summary>
        /// 數學行動變數
        /// </summary>
        public MathQuestioningActionParameters Parameters => parameters;
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
        /// <summary>
        /// 正在答題中
        /// </summary>
        public bool IsQuestioning => isQuestioning;

        #endregion
        
        
        #region 暫存 Cache
        
        
        [SerializeField] private MathQuestioningActionParameters parameters;
        /// <summary>
        /// 題目清單
        /// </summary>
        [SerializeField] private List<MultipleChoiceQuestion> questionList;
        /// <summary>
        /// 正在回答的題目
        /// </summary>
        private MultipleChoiceQuestion _currentQuestion;
        /// <summary>
        /// 正確題目
        /// </summary>
        private int _correctAnswer;
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
        
        
        /// <summary>
        /// 這次進入答題介面的答題紀錄
        /// </summary>
        [SerializeField]  private AnswerRecord answerRecord;
        /// <summary>
        /// 這場戰鬥的答題記錄
        /// </summary>
        [SerializeField] private AnswerRecord combatAnswerRecord;

        /// <summary>
        /// 是否播放答題正確的行動
        /// </summary>
        private bool _playCorrectAction;
        

        #endregion
        
        #region Setup
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                transform.parent = null;
                Instance = this;
            }
            
            isQuestioning = false;
            questionController.SetQuestionManager(this);
            parameters = new MathQuestioningActionParameters();
        }

        #endregion


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
            EventManager.OnAnswer?.Invoke();
            
            Debug.Log($"option {option } correctAnswer {_correctAnswer}");
            
            if (option == _correctAnswer)
            {
                answerRecord.CorrectCount++;
                combatAnswerRecord.CorrectCount++;
                questionController.OnAnswer(true, option);
                EventManager.OnAnswerCorrect?.Invoke();
            }
            else
            {
                answerRecord.WrongCount++;
                combatAnswerRecord.WrongCount++;
                questionController.OnAnswer(false, option);
                EventManager.OnAnswerWrong?.Invoke();
            }

            answerRecord.AnswerCount++;
            combatAnswerRecord.AnswerCount++;
            
            waitAnswer = false;
            
        }

        

        #endregion
        

        #region Public Method
        
        /// <summary>
        /// 進入答題模式
        /// </summary>
        /// <param name="newParameters"></param>
        public void EnterQuestionMode(MathQuestioningActionParameters newParameters)
        {
            parameters = newParameters;
            StartCoroutine(QuestionCoroutine());
        }
        

        public AnswerButtonBase GetAnswerButton(int option)
        {
            if (option >= answerButtons.Length)
            {
                Debug.LogError($"{option} 超過 answerbuttons 數量");
                return null;
            }

            return answerButtons[option];
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

        #endregion
        
        #region Questioning Coroutine
        
        IEnumerator QuestionCoroutine()
        {
            // Init
            answerRecord.Clear();
            isQuestioning = true;
            
            GenerateQuestions();
            if (CollectionManager)
                CollectionManager.HandController.DisableDragging();

            
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
            EnableDragging();
            // isQuestioning = false;
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
            _correctAnswer = _currentQuestion.Answer;
            questionList.RemoveAt(index);
        }

        void GenerateQuestions()
        {
            questionList = new List<MultipleChoiceQuestion>(questionsData.MultipleChoiceQuestions);
        }
        
        private void PlayAfterQuestioningAction()
        {
            if (parameters.QuestioningEndJudgeType == QuestioningEndJudgeType.LimitedQuestionCount)
            {
                GameActionExecutor.Instance.AddToBottom(parameters.LimitedQuestionAction);
            }
            else
            {
                if (parameters.UseCorrectAction && _playCorrectAction)
                {
                    GameActionExecutor.Instance.AddToBottom(parameters.CorrectActions);
                }

                if (parameters.UseWrongAction && !_playCorrectAction)
                {
                    GameActionExecutor.Instance.AddToBottom(parameters.WrongActions);
                }
            }
            
        }
        
        private void EnableDragging()
        {
            if (CollectionManager)
                CollectionManager.HandController.EnableDragging();
        }

        
        #endregion
        
        #region Judge End Questioning Mode Condition
        private void JudgeEndConditions()
        {
            if (parameters.QuestioningEndJudgeType == QuestioningEndJudgeType.LimitedQuestionCount)
            {
                JudgeHasAnsweredQuestionCount();
            }
            else if (parameters.QuestioningEndJudgeType == QuestioningEndJudgeType.CorrectOrWrongCount)
            {
                JudgeCorrectCount();
                JudgeWrongCount();
            }
        }
        

        private void JudgeHasAnsweredQuestionCount()
        {
            
            if (answerRecord.AnswerCount >= parameters.QuestionCount)
            {
                ExitQuestionMode("魔法詠唱結束");
            }
            
        }

        private void JudgeCorrectCount()
        {
            if (parameters.UseCorrectAction && answerRecord.CorrectCount >= parameters.CorrectActionNeedAnswerCount)
            {
                _playCorrectAction = true;
                ExitQuestionMode("魔法詠唱成功，發動好效果");

            }
        }
        
        private void JudgeWrongCount()
        {
            if (parameters.UseWrongAction && answerRecord.WrongCount >= parameters.WrongActionNeedAnswerCount)
            {
                _playCorrectAction = false;
                ExitQuestionMode("魔法詠唱失敗，發動壞效果");
            }
        }
        
        void ExitQuestionMode(string info)
        {
            isQuestioning = false;
            questionController.ExitQuestionMode(info);
        }
        
        #endregion
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
