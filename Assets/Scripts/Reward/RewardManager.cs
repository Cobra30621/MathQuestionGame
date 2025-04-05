using System;
using System.Collections.Generic;
using Card.Data;
using Managers;
using Map;
using NueGames.Data.Containers;
using Question;
using Relic.Data;
using Reward.Data;
using Sirenix.OdinInspector;
using Stage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Reward
{
    public class RewardManager : MonoBehaviour
    {
        [Required]
        [InlineEditor]
        public DeckData commonCardDeck;

        [Required]
        [InlineEditor]
        public ItemDropData ItemDropData;

        [Required]
        [InlineEditor]
        public RewardContainerData RewardContainerData; 
        
        public static RewardManager Instance => GameManager.Instance.RewardManager;


        public Sprite GetRewardSprite(RewardType rewardType)
        {
            return RewardContainerData.RewardsSprites[rewardType];
        }


        public List<CardData> GetCombatWinCardList(int amount)
        {
            var cards = new List<CardData>();
            for (int i = 0; i < amount; i++)
            {
                CardData card;
                // 一定機率獲得通用卡牌
                if (Random.Range(0f, 1f) < ItemDropData.commonCardDropRate)
                {
                    card = GetCard(new RewardData()
                    {
                        ItemGainType = ItemGainType.Common
                    });
                }
                else
                {
                    card = GetCard(new RewardData()
                    {
                        ItemGainType = ItemGainType.Character
                    });
                }
                
                cards.Add(card);
            }
            
            return cards;
        }
        
        public List<CardData> GetCardList(RewardData rewardData, int amount)
        {
            var cards = new List<CardData>();
            for (int i = 0; i < amount; i++)
            {
                var card = GetCard(rewardData);
                cards.Add(card);
            }
            
            return cards;
        }
        
        private CardData GetCard(RewardData rewardData)
        {
            switch (rewardData.ItemGainType)
            {
                case ItemGainType.Character:
                    var characterDeck = StageSelectedManager.Instance.GetAllyData().CardRewardData;
                    return characterDeck.GetRandomCard();
                case ItemGainType.Common:
                    return commonCardDeck.GetRandomCard();
                case ItemGainType.Specify:
                    return rewardData.specifyCard;
                default:
                    Debug.LogError("Unknown reward");
                    return null;
            }
        }

        public int GetMoney(RewardData rewardData, NodeType nodeType)
        {
            int basicCoin = 0;
            var moneyDropRate = GameManager.Instance.GetMoneyDropRate();
            
            if (rewardData.CoinGainType == CoinGainType.Specify)
            {
                basicCoin = rewardData.specifyCoin;
            }
            else
            {
                basicCoin = ItemDropData.GetNodeDropMoney(nodeType);
            }

            return (int) Math.Floor(basicCoin * moneyDropRate);
        }

        public int GetStone(RewardData rewardData, NodeType nodeType)
        {
            int basicStone = 0;
            float stoneDropRate = 1f;
            
            if (rewardData.CoinGainType == CoinGainType.Specify)
            {
                basicStone = rewardData.specifyCoin;
            }
            else
            {
                basicStone = ItemDropData.GetNodeDropStone(nodeType);
            }

            return (int) Math.Floor(basicStone * stoneDropRate);
        }

        public int GetQuestionReward(AnswerRecord record)
        {
            var questionDropStone = ItemDropData.questionDropStone;

            var stone = record.CorrectCount * questionDropStone;

            return stone;
        }

        public (RelicName, RelicData) GetRelic(NodeType nodeType, RewardData rewardData)
        {
            bool isSpecified = rewardData.ItemGainType == ItemGainType.Specify;
            if (isSpecified)
            {
                return ItemDropData.GetRelicData(rewardData.specifyRelic);
            }
            
            return ItemDropData.GetRelicData(nodeType);
        }
    }
}