using System;
using System.Collections.Generic;
using NueGames.Event.Effect;
using Question;
using Reward;
using Sirenix.OdinInspector;

namespace NueGames.Event
{
    [Serializable]
    public class EffectData
    {
        [LabelText("效果類型")]
        public EffectType EffectType;

        [LabelText("獎勵")]
        [ShowIf("IsShowRewardData")]
        public List<RewardData> RewardData;

        [LabelText("問題難度")]
        [ShowIf("IsShowQuestionMode")]
        public QuestionMode QuestionMode;

        [LabelText("血量調整數值")] [ShowIf("IsShowChangeHeartData")]
        public int changeHealthValue;
        

        private bool IsShowRewardData()
        {
            return EffectType == EffectType.Reward || EffectType == EffectType.MathQuestion;
        }

        private bool IsShowQuestionMode()
        {
            return EffectType == EffectType.MathQuestion;
        }

        private bool IsShowChangeHeartData()
        {
            return EffectType == EffectType.ChangeHealth;
        }
    }
}