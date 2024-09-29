using System.Collections.Generic;
using Managers;
using Map;
using NueGames.Event;
using Reward;

namespace Question.QuestionAction
{
    public class EventQuestionAction : QuestionActionBase
    {
        public List<RewardData> RewardData;

        public System.Action onComplete;
        
        public override void DoCorrectAction()
        {
            UIManager.Instance.RewardCanvas.ShowReward(RewardData, NodeType.Event, false);
            
            onComplete.Invoke();
        }

        public override void DoWrongAction()
        {
            onComplete.Invoke();
        }
    }
}