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
    public class QuestionManager : MonoBehaviour
    {
        private QuestionManager(){}
        public static QuestionManager Instance { get; private set; }
        [SerializeField] private QuestionController questionController;
        
        [SerializeField] private QuestionsData questionsData;
        [SerializeField] private AnswerButton[] answerButtons;

        private CollectionManager CollectionManager => CollectionManager.Instance;
        private EventManager EventManager => EventManager.Instance;
        
        public float Timer{
            get{
                if(timer < 0)
                    return 0;
                else
                    return timer;
            }
        }

        public int HasAnswerCount => answerRecord.AnswerCount;
        public int CorrectAnswerCount => answerRecord.CorrectCount;
        public int WrongAnswerCount => answerRecord.WrongCount;
        public bool IsQuestioning => isQuestioning;
        public MathQuestioningActionParameters Parameters => parameters;
        
        
        #region Cache
        [SerializeField] private MathQuestioningActionParameters parameters;
        
        private MultipleChoiceQuestion currentQuestion;
        [SerializeField] private List<MultipleChoiceQuestion> questionList;
        private int correctAnswer;

        private float timer; 
        [SerializeField] private bool timeOver;
        [SerializeField] private bool isQuestioning;
        private bool waitAnswer;
        [SerializeField] private bool isPlayingFeedback;

        /// <summary>
        /// 這次進入答題介面的答題紀錄
        /// </summary>
        [SerializeField]  private AnswerRecord answerRecord;
        /// <summary>
        /// 這場戰鬥的答題記錄
        /// </summary>
        [SerializeField] private AnswerRecord combatAnswerRecord;

        private bool playCorrectAction;
        
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
                DontDestroyOnLoad(gameObject);
            }
            
            timeOver = true;
            isQuestioning = false;
            questionController.SetQuestionManager(this);
            parameters = new MathQuestioningActionParameters();
        }

        #endregion

        #region Update
        void Update()
        {
            Countdown(); 
            
            // JudgeEndConditions();
        }
        
        void Countdown(){
            if(!parameters.UseTimeCountDown){return;}
            if(timeOver){return;}
            
            timer -= Time.deltaTime;
        }
        

        #endregion

        #region Public Method
        /// <summary>
        /// 戰鬥開始時的初始化
        /// </summary>
        public void OnCombatStart()
        {
            combatAnswerRecord.Clear();
        }
        
        public void EnterQuestionMode(MathQuestioningActionParameters newParameters)
        {
            parameters = newParameters;
            StartCoroutine(QuestionCoroutine());
        }

        public void OnAnswer(int option)
        {
            EventManager.OnAnswer?.Invoke();
            if (option == correctAnswer)
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
            EnableAnswer(false);
        }

        public void SetPlayingFeedback(bool isPlaying)
        {
            isPlayingFeedback = isPlaying;
        }

        public MMF_Player GetAnswerFeedback(bool correct, int option)
        {
            if (option >= answerButtons.Length)
            {
                Debug.LogError($"{option} 超過 answerbuttons 數量");
                return null;
            }
            
            AnswerButton answerButton = answerButtons[option];
            if (correct)
                return answerButton.CorrectFeedback;
            else
                return answerButton.WrongFeedback;
        }

        public void EnableDragging()
        {
            if (CollectionManager)
                CollectionManager.HandController.EnableDragging();
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
            timer = parameters.Time;
            timeOver = false;

            answerRecord.Clear();
            isQuestioning = true;
            
            GenerateQuestions();
            if (CollectionManager)
                CollectionManager.HandController.DisableDragging();
            
            
            questionController.EnterQuestionMode();
            yield return new WaitForSeconds(0.2f);
            while (isPlayingFeedback) yield return null; // 等待開頭反饋特效

            // Start Questioning
            while (isQuestioning)
            {
                NextQuestion();
                questionController.SetNextQuestion(currentQuestion);
                while (isPlayingFeedback) yield return null; // 等待顯示題目反饋特效
                
                EnableAnswer(true);
                waitAnswer = true;
                while (waitAnswer) yield return null;
                yield return new WaitForSeconds(0.5f);
                while (isPlayingFeedback) yield return null; // 等待答題成功反饋特效
                
                JudgeEndConditions();
            }
            
            while (isPlayingFeedback) yield return null; // 等待離開動畫反饋特效
            yield return new WaitForSeconds(0.1f);

            PlayAfterQuestioningAction();

        }
        
        void EnableAnswer(bool enable)
        {
            foreach (AnswerButton answerButton in answerButtons)
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
            currentQuestion = questionList[index];
            correctAnswer = currentQuestion.Answer;
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
                if (parameters.UseCorrectAction && playCorrectAction)
                {
                    GameActionExecutor.Instance.AddToBottom(parameters.CorrectActions);
                }

                if (parameters.UseWrongAction && !playCorrectAction)
                {
                    GameActionExecutor.Instance.AddToBottom(parameters.WrongActions);
                }
            }
            
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
        
        private void JudgeTimeEnd()
        {
            if(!parameters.UseTimeCountDown){return;}
            
            if(timer < 0)
            {
                ExitQuestionMode("魔法詠唱失敗，發動壞效果");
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
                playCorrectAction = true;
                ExitQuestionMode("魔法詠唱成功，發動好效果");

            }
        }
        
        private void JudgeWrongCount()
        {
            if (parameters.UseWrongAction && answerRecord.WrongCount >= parameters.WrongActionNeedAnswerCount)
            {
                playCorrectAction = false;
                ExitQuestionMode("魔法詠唱失敗，發動壞效果");
            }
        }
        
        void ExitQuestionMode(string info)
        {
            timeOver = true;
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
