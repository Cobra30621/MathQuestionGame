using System.Collections.Generic;
using Managers;
using Map;
using Reward;
using Reward.Data;
using UI;

namespace Question.Action
{
    public class EventQuestionAction : QuestionActionBase
    {
        public List<RewardData> RewardData;

        public System.Action onComplete;
        
        public override void DoAnswerCompeled()
        {
            UIManager.Instance.RewardCanvas.ShowReward(RewardData, NodeType.Event, false, onComplete);
            
        }


    }
}