using NueGames.NueDeck.Scripts.Card;
using Question;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using NueGames.NueDeck.Scripts.Action.MathAction;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

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

        public int HasAnswerCount => hasAnswerCount;
        public int CorrectAnswerCount => correctAnswerCount;
        public int WrongAnswerCount => wrongAnswerCount;
        public bool IsQuestioning => isQuestioning;
        public MathQuestioningActionParameters Parameters => parameters;
        
        
        #region Cache
        [SerializeField] private MathQuestioningActionParameters parameters;
        
        private MultipleChoiceQuestion currentQuestion;
        private List<MultipleChoiceQuestion> questionList;
        private int correctAnswer;

        private float timer; 
        [SerializeField] private bool timeOver;
        [SerializeField] private bool isQuestioning;
        private bool waitAnswer;
        [SerializeField] private bool isPlayingFeedback;

        [SerializeField] private int hasAnswerCount;
        [SerializeField] private int correctAnswerCount;
        [SerializeField]  private int wrongAnswerCount;
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
        public void EnterQuestionMode(MathQuestioningActionParameters newParameters)
        {
            parameters = newParameters;
            StartCoroutine(QuestionCoroutine());
        }

        public void OnAnswer(int option)
        {
            EventManager.OnAnswer();
            if (option == correctAnswer)
            {
                correctAnswerCount++;
                questionController.OnAnswer(true, option);
                EventManager.OnAnswerCorrect();
            }
            else
            {
                wrongAnswerCount++;
                questionController.OnAnswer(false, option);
                EventManager.OnAnswerWrong();
            }

            hasAnswerCount++;
            
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

        #endregion
        
        #region Questioning Coroutine
        
        IEnumerator QuestionCoroutine()
        {
            // Init
            timer = parameters.Time;
            timeOver = false;

            hasAnswerCount = 0;
            correctAnswerCount = 0;
            wrongAnswerCount = 0;
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
            
            if (hasAnswerCount >= parameters.QuestionCount)
            {
                ExitQuestionMode("魔法詠唱結束");
            }
            
        }

        private void JudgeCorrectCount()
        {
            if (parameters.UseCorrectAction && correctAnswerCount >= parameters.CorrectActionNeedAnswerCount)
            {
                playCorrectAction = true;
                ExitQuestionMode("魔法詠唱成功，發動好效果");

            }
        }
        
        private void JudgeWrongCount()
        {
            if (parameters.UseWrongAction && wrongAnswerCount >= parameters.WrongActionNeedAnswerCount)
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
}
