
using System.Collections.Generic;
using Data.Settings;
using Map;
using NueGames.Data.Collection;
using NueGames.Data.Collection.RewardData;
using NueGames.NueExtentions;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Data.Containers
{
    [CreateAssetMenu(fileName = "Reward Container", menuName = "NueDeck/Containers/Reward", order = 4)]
    public class RewardContainerData : ScriptableObject
    {
        [Required]
        [SerializeField] private CardRewardData cardRewardData;
        [Required]
        [SerializeField] private GoldRewardData goldRewardData;
        [Required]
        [SerializeField] private ItemDropData _itemDropData;
        
        public CardRewardData CardRewardData => cardRewardData;
        public GoldRewardData GoldRewardData => goldRewardData;

        public void SetCardRewardData(CardRewardData data)
        {
            cardRewardData = data;
        }
        
        public int GetRandomGoldReward(NodeType nodeType)
        {
            return _itemDropData.GetNodeDropMoney(nodeType);
        } 
       
    }

}