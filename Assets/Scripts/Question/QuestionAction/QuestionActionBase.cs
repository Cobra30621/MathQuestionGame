using Action.Parameters;

namespace Question.QuestionAction
{
    public abstract class QuestionActionBase
    {
        public abstract void DoCorrectAction();
        public abstract void DoWrongAction();

        public ActionSource GetActionSource()
        {
            return new ActionSource();
        }
    }
}