using Question.Enum;

namespace Question.Action
{
    /// <summary>
    /// 數學答題相關的行動
    /// </summary>
    public abstract class QuestionActionBase
    {
        /// <summary>
        /// 出題數量
        /// </summary>
        public int QuestionCount;
        
        /// <summary>
        /// 當網路不佳下載失敗時是否改用本地資料
        /// </summary>
        public bool fallbackToLocalIfNoInternet = true;
        
        /// <summary>
        /// 答題結束後的行動
        /// </summary>
        public abstract void DoAnswerCompeled();

    }
}