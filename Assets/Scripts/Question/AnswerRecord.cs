using System;

namespace Question
{
    /// <summary>
    /// 答題紀錄
    /// </summary>
    [Serializable]
    public class AnswerRecord
    {
        public int QuestionCount;
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