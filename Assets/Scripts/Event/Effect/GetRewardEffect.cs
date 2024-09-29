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
    /// <summary>
    /// This class represents an effect that grants rewards when triggered.
    /// </summary>
    public class GetRewardEffect : IEffect
    {
        [SerializeField] private List<RewardData> _rewardData; // Stores the rewards to be granted.

        /// <summary>
        /// Initializes the effect with the provided effect data.
        /// </summary>
        /// <param name="effectData">The data used to initialize the effect.</param>
        public void Init(EffectData effectData)
        {
            _rewardData = effectData.RewardData;
        }

        /// <summary>
        /// Executes the effect, showing the rewards to the player.
        /// </summary>
        public void Execute(System.Action onComplete)
        {
            UIManager.Instance.RewardCanvas.ShowReward(_rewardData, NodeType.Event, false, onComplete);
      
        }

        

        /// <summary>
        /// Checks if the effect is selectable.
        /// </summary>
        /// <returns>True if the effect is selectable, false otherwise.</returns>
        public bool IsSelectable()
        {
            return true;
        }
    }
}