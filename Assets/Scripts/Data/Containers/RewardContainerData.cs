
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
        [SerializeField] private List<CardRewardData> cardRewardDataList;
        [SerializeField] private List<GoldRewardData> goldRewardDataList;
        public List<CardRewardData> CardRewardDataList => cardRewardDataList;
        public List<GoldRewardData> GoldRewardDataList => goldRewardDataList;

        public List<CardData> GetRandomCardRewardList(out CardRewardData rewardData)
        {
            rewardData = CardRewardDataList.RandomItem();
            
            List<CardData> cardList = new List<CardData>();
            
            foreach (var cardData in rewardData.RewardCardList)
                cardList.Add(cardData);

            return cardList;
        } 
        public int GetRandomGoldReward(out GoldRewardData rewardData)
        { 
            rewardData = GoldRewardDataList.RandomItem();
            var value =Random.Range(rewardData.MinGold, rewardData.MaxGold);

            return value;
        } 
       
    }

}