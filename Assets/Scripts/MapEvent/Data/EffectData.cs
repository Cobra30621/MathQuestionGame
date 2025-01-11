using System;
using System.Collections.Generic;
using MapEvent.Effect;
using Question;
using Question.Enum;
using Reward;
using Reward.Data;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace MapEvent.Data
{
    [Serializable]
    public class EffectData
    {
        [FormerlySerializedAs("EffectType")] [LabelText("效果類型")]
        public EventEffectType eventEffectType;

        [LabelText("獎勵")]
        [ShowIf("IsShowRewardData")]
        public List<RewardData> RewardData;

        [LabelText("問題難度")]
        [ShowIf("IsShowQuestionMode")]
        public QuestionMode QuestionMode;

        [LabelText("血量調整數值")] [ShowIf("IsShowChangeHeartData")]
        public int changeHealthValue;
        
        [LabelText("支付獲得獎勵")]
        [ShowIf("IsShowPayAndGainData")]
        public PayAndGainData PayAndGainData;
        

        private bool IsShowRewardData()
        {
            return eventEffectType == EventEffectType.Reward || eventEffectType == EventEffectType.MathQuestion;
        }

        private bool IsShowQuestionMode()
        {
            return eventEffectType == EventEffectType.MathQuestion;
        }

        private bool IsShowChangeHeartData()
        {
            return eventEffectType == EventEffectType.ChangeHealth;
        }
        
        private bool IsShowPayAndGainData()
        {
            return eventEffectType == EventEffectType.PayAndGain;
        }
    }
}