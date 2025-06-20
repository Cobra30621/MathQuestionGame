namespace Question.Action
{
    /// <summary>
    /// 在主介面進行答題
    /// </summary>
    public class NormalQuestionAction : QuestionActionBase
    {
        public override void DoAnswerCompeled()
        {
            QuestionManager.Instance.ShowOutcome();
        }
    }
}