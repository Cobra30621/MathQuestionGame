using System;
using System.Collections.Generic;
using NueGames.Event.Effect;
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
        public  List<RewardData> RewardData;
    }
}