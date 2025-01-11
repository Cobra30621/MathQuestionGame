using MapEvent.Data;
using Question;
using Question.Action;

namespace MapEvent.Effect
{
    public class QuestionEffect : IEffect
    {
        private EffectData _effectData;
        
        public void Init(EffectData effectData)
        {
            _effectData = effectData;
        }

        public void Execute(System.Action onComplete)
        {
            var questionAction = new EventQuestionAction()
            {
                QuestionCount = 1,
                NeedCorrectCount = 1,
                QuestionMode = _effectData.QuestionMode,
                RewardData = _effectData.RewardData,
                onComplete =  onComplete
            };
            
            QuestionManager.Instance.EnterQuestionMode(questionAction);
        }


        public bool IsSelectable()
        {
            return true;
        }
    }
}