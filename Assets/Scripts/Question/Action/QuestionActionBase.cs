using Action.Parameters;

namespace Question.QuestionAction
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
        /// 需要答對的題數
        /// </summary>
        public int NeedCorrectCount;
        
        /// <summary>
        /// 問題難度
        /// </summary>
        public QuestionMode QuestionMode;
        
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