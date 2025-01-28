namespace Question.Action
{
    /// <summary>
    /// 在主介面進行答題
    /// </summary>
    public class NormalQuestionAction : QuestionActionBase
    {
        public override void DoCorrectAction()
        {
            QuestionManager.Instance.ShowOutcome();
        }

        public override void DoWrongAction()
        {
            QuestionManager.Instance.ShowOutcome();
        }
    }
}