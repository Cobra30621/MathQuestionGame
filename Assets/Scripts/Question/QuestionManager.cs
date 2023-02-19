using NueGames.NueDeck.Scripts.Card;
using NueGames.Card.CardActions;
using Question;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;

namespace Question
{
    public class QuestionManager : MonoBehaviour
    {
        private QuestionManager(){}
        public static QuestionManager Instance { get; private set; }
        [SerializeField] private QuestionController questionController;
        
        [SerializeField] private QuestionsData questionsData;
        
        public float Timer{
            get{
                if(timer < 0)
                    return 0;
                else
                    return timer;
            }
        }
        public float StartTime =>  startTime;
        public int CorrectCount => correctCount;
        public int WrongCount => wrongCount;
        public int CorrectActionNeedAnswerCount => correctActionNeedAnswerCount;
        public int WrongActionNeedAnswerCount => wrongActionNeedAnswerCount;
        public bool IsPlayingFeedback => isPlayingFeedback;
        [SerializeField] private AnswerButton[] answerButtons;

        #region Cache
        
        private CardActionParameters tempCardActionParameters;
        private MathActionBase tempMathAction;
        private MultipleChoiceQuestion currentQuestion;
        private List<MultipleChoiceQuestion> questionList;
        private int correctAnswer;

        private float startTime = 30;
        private float timer; 
        [SerializeField] private bool timeOver;
        private bool questioning;
        private bool waitAnswer;
        [SerializeField] private bool isPlayingFeedback;
        private int correctActionNeedAnswerCount;
        private int wrongActionNeedAnswerCount;
        private int correctCount;
        private int wrongCount;
        
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
            questioning = false;
            questionController.SetQuestionManager(this);
        }

        #endregion
        
        #region Public Methods

        public void EnterQuestion(MathActionBase mathAction, int correctNeedAnswerCount, int wrongNeedAnswerCount)
        {
            tempMathAction = mathAction;
            correctActionNeedAnswerCount = correctNeedAnswerCount;
            wrongActionNeedAnswerCount = wrongNeedAnswerCount;
            
            questionController.EnterQuestionMode();
            StartCoroutine(QuestionCoroutine());
        }

        public void StartQuestion()
        {
            correctActionNeedAnswerCount =10;
            wrongActionNeedAnswerCount = 10;
            questionController.EnterQuestionMode();
            StartCoroutine(QuestionCoroutine());
        }
        

        public void SetPlayingFeedback(bool isPlaying)
        {
            isPlayingFeedback = isPlaying;
        }

        public void OnAnswer(int option)
        {
            if (option == correctAnswer)
            {
                correctCount++;
                questionController.OnAnswer(true, option);
            }
            else
            {
                wrongCount++;
                questionController.OnAnswer(false, option);
            }
            waitAnswer = false;
            EnableAnswer(false);
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
        
        #endregion
        
        #region Private Methods
        
        IEnumerator QuestionCoroutine()
        {
            // Init
            timer = startTime;
            timeOver = false;
            correctCount = 0;
            wrongCount = 0;
            questioning = true;
            InitQuestionList();
            yield return new WaitForSeconds(0.5f);
            while (isPlayingFeedback) yield return null; // 等待開頭反饋特效

            // Start Questioning
            while (questioning)
            {
                NextQuestion();
                questionController.SetNextQuestion(currentQuestion);
                while (isPlayingFeedback) yield return null; // 等待顯示題目反饋特效
                EnableAnswer(true);
                waitAnswer = true;
                while (waitAnswer) yield return null;
                yield return new WaitForSeconds(0.5f);
                while (isPlayingFeedback) yield return null; // 等待答題成功反饋特效

            }
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
                InitQuestionList();
            }
            
            int index = new System.Random().Next(questionList.Count);
            currentQuestion = questionList[index];
            correctAnswer = currentQuestion.Answer;
            questionList.RemoveAt(index);
        }

        void InitQuestionList()
        {
            questionList = new List<MultipleChoiceQuestion>(questionsData.MultipleChoiceQuestions);
        }
        

        void Update()
        {
            JudgeMathCardAction();
            // Countdown();
        }
        
        void Countdown(){
            if(timeOver){return;}
            
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                tempMathAction.OnAnswer(false);
                ExitQuestionMode();
            }

            
        }

        void JudgeMathCardAction()
        {
            if (!questioning) { return;}
            
            if (correctCount >= correctActionNeedAnswerCount)
            {
                tempMathAction.OnAnswer(true);
                ExitQuestionMode();
            }
            
            if (wrongCount >= wrongActionNeedAnswerCount)
            {
                tempMathAction.OnAnswer(false);
                Debug.Log("Wrong");
                ExitQuestionMode();
            }
        }

        void ExitQuestionMode()
        {
            Debug.Log("End");
            timeOver = true;
            questioning = false;
            questionController.ExitQuestionMode();
            StopCoroutine(QuestionCoroutine());
            Debug.Log("End2");
        }
        
        
        #endregion
    }
}
