using System.Collections.Generic;
using Question.Action;
using Question.Data;
using UnityEngine;

namespace Question.Core
{
    /// <summary>
    /// 管理單次答題階段的狀態和記錄
    /// </summary>
    public class QuestionSession
    {
        private AnswerRecord _answerRecord = new();
        private Data.Question _currentQuestion;
        
        private List<Data.Question> rawQuestionList = new();
        
        private List<Data.Question> _questionList = new();

        public AnswerRecord AnswerRecord => _answerRecord;
        
        /// <summary>
        /// 本次數學答題結束後的行動
        /// </summary>
        private QuestionActionBase questionAction;
        
        public int HasAnswerCount => _answerRecord.AnswerCount;
        public int CorrectAnswerCount => _answerRecord.CorrectCount;
        public int WrongAnswerCount => _answerRecord.WrongCount;
        public int QuestionCount => questionAction.QuestionCount;
        
        
        public Data.Question CurrentQuestion => _currentQuestion;

        public void StartNewSession(QuestionActionBase questionAction)
        {
            this.questionAction = questionAction;
            _answerRecord.Clear();
        }

        public void SetQuestions(List<Data.Question> questions)
        {
            rawQuestionList = questions;
        }
        
        
        public void RecordAnswer(bool isCorrect)
        {
            if (isCorrect)
            {
                _answerRecord.CorrectCount++;
            }
            else
            {
                _answerRecord.WrongCount++;
            }

            _answerRecord.AnswerCount++;
        }
        public Data.Question GetNextQuestion()
        {
            Debug.Log($"Get Question {_questionList.Count}");
            if (_questionList.Count == 0)
            {
                _questionList.AddRange(rawQuestionList);
            }
            
            int index = new System.Random().Next(_questionList.Count);
            _currentQuestion = _questionList[index];
            _questionList.RemoveAt(index);
            return _currentQuestion;
        }
        
        public bool IsFinishAllQuestion()
        {
            return _answerRecord.AnswerCount >= questionAction.QuestionCount;
        }
        
        public bool ReachSuccessCondition()
        {
            return _answerRecord.CorrectCount >= questionAction.NeedCorrectCount;
        }
    }
}