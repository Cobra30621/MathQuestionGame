using NueGames.NueDeck.Scripts.Card;
using NueGames.Card.CardActions;
using Question;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Question
{
    public class QuestionManager : MonoBehaviour
    {
        private QuestionManager(){}
        public static QuestionManager Instance { get; private set; }
        [SerializeField] private QuestionController questionController;
        [SerializeField] private GameObject mainPanel;
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
        public int NeedCorrectCount => needCorrectCount;

        #region Cache
        
        private CardActionParameters tempCardActionParameters;
        private MathActionBase tempMathAction;
        private MultipleChoiceQuestion currentQuestion;
        private List<MultipleChoiceQuestion> questionList;
        private int correctAnswer;

        private float startTime = 10;
        private float timer; 
        private bool timeOver;
        private bool waitAnswer;

        private int needCorrectCount = 2;
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
            
            questionController.SetQuestionManager(this);
        }

        #endregion
        
        #region Public Methods

        public void EnterQuestion(MathActionBase mathAction)
        {
            tempMathAction = mathAction;
            mainPanel.SetActive(true);
            StartCoroutine(QuestionCoroutine());
        }

        public void OnAnswer(int option)
        {
            if (option == correctAnswer)
            {
                correctCount++;
            }
            else
            {
                wrongCount++;
            }
            waitAnswer = false;
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
            InitQuestionList();

            // Start Questioning
            while (!timeOver)
            {
                NextQuestion();
                questionController.SetNextQuestion(currentQuestion);
                waitAnswer = true;
                while (waitAnswer) yield return null;

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
            Countdown();
        }
        
        void Countdown(){
            if(timeOver){return;}

            timer -= Time.deltaTime;
            if(timer < 0){
                timeOver = true;
                StopCoroutine(QuestionCoroutine());
                PlayAction();
            }
        }

        void PlayAction()
        {
            if (correctCount >= 2)
            {
                tempMathAction.OnAnswer();
            }
            mainPanel.SetActive(false);
        }
        
        #endregion
    }
}
