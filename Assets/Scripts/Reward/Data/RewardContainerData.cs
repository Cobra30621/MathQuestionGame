using System.Collections.Generic;
using Reward;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Containers
{
    [CreateAssetMenu(fileName = "Reward Container", menuName = "NueDeck/Containers/Reward", order = 4)]
    public class RewardContainerData : SerializedScriptableObject
    {
        [LabelText("獎勵圖片集")]
        public Dictionary<RewardType, Sprite> RewardsSprites;
    }

}