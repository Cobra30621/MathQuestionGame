using System.Collections.Generic;
using Managers;
using Map;
using NueGames.Data.Collection.RewardData;
using NueGames.Enums;
using NueGames.UI.Reward;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Event.Effect
{
    public class GetCardEffect : IEffect
    {
        public void Execute()
        {
            UIManager.Instance.RewardCanvas.ShowReward(new List<RewardType>()
            {
                RewardType.Card
            }, NodeType.Event);
        }

        public bool IsSelectable()
        {
            return true;
        }
    }
}