
using System.Collections.Generic;
using NueGames.Data.Collection;
using NueGames.Data.Collection.RewardData;
using NueGames.NueExtentions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NueGames.Data.Containers
{
    [CreateAssetMenu(fileName = "Reward Container", menuName = "NueDeck/Containers/Reward", order = 4)]
    public class RewardContainerData : ScriptableObject
    {
        [SerializeField] private CardRewardData cardRewardData;
        [SerializeField] private List<GoldRewardData> goldRewardDataList;
        public CardRewardData CardRewardData => cardRewardData;
        public List<GoldRewardData> GoldRewardDataList => goldRewardDataList;

        public void SetCardRewardData(CardRewardData data)
        {
            cardRewardData = data;
        }
        
        public int GetRandomGoldReward(out GoldRewardData rewardData)
        { 
            rewardData = GoldRewardDataList.RandomItem();
            var value =Random.Range(rewardData.MinGold, rewardData.MaxGold);

            return value;
        } 
       
    }

}