using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Reward.Data
{
    [CreateAssetMenu(fileName = "Reward Container", menuName = "NueDeck/Containers/Reward", order = 4)]
    public class RewardContainerData : SerializedScriptableObject
    {
        [LabelText("獎勵圖片集")]
        public Dictionary<RewardType, Sprite> RewardsSprites;
    }

}