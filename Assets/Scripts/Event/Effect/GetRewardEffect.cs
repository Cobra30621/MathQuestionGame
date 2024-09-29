using System.Collections.Generic;
using Managers;
using Map;
using NueGames.Data.Collection.RewardData;
using NueGames.Enums;
using NueGames.UI.Reward;
using Reward;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Event.Effect
{
    public class GetRewardEffect : IEffect
    {
        [SerializeField] private List<RewardData> _rewardData;
        public void Init(EffectData effectData)
        {
            _rewardData = effectData.RewardData;
        }

        public void Execute()
        {
            UIManager.Instance.RewardCanvas.ShowReward(_rewardData, NodeType.Event, 
                false);
        }

        public bool IsSelectable()
        {
            return true;
        }
    }
}