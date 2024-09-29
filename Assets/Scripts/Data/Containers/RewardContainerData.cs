
using System.Collections.Generic;
using Data.Settings;
using Map;
using NueGames.Data.Collection;
using NueGames.Data.Collection.RewardData;
using NueGames.NueExtentions;
using Reward;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Data.Containers
{
    [CreateAssetMenu(fileName = "Reward Container", menuName = "NueDeck/Containers/Reward", order = 4)]
    public class RewardContainerData : SerializedScriptableObject
    {
        [LabelText("獎勵圖片集")]
        public Dictionary<RewardType, Sprite> RewardsSprites;
    }

}