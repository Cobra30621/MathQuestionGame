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
        /// 需要答對的題數
        /// </summary>
        public int NeedCorrectCount;
        
        /// <summary>
        /// 答題成功的行動
        /// </summary>
        public abstract void DoCorrectAction();
        /// <summary>
        /// 答題失敗的行動
        /// </summary>
        public abstract void DoWrongAction();

    }
}