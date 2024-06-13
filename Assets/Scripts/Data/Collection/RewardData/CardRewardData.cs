using System.Collections.Generic;
using Card.Data;
using UnityEngine;

namespace NueGames.Data.Collection.RewardData
{
    [CreateAssetMenu(fileName = "Card Reward Data",menuName = "NueDeck/Collection/Rewards/CardRW",order = 0)]
    public class CardRewardData : RewardDataBase
    {
        [SerializeField] private List<CardData> rewardCardList;
        public List<CardData> RewardCardList => rewardCardList;
    }
}