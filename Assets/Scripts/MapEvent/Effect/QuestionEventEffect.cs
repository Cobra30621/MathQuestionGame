using MapEvent.Data;
using Question;
using Question.Action;

namespace MapEvent.Effect
{
    public class QuestionEventEffect : IEventEffect
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
                RewardData = _effectData.RewardData,
                onComplete =  onComplete
            };
            
            QuestionManager.Instance.EnterQuestionMode(questionAction, 1);
        }


        public bool IsSelectable()
        {
            return true;
        }
    }
}