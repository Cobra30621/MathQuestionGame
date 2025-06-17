using System;
using System.Collections.Generic;
using UnityEngine;
using Card.Data;
using Managers;
using Map;
using NueGames.Data.Containers;
using Question;
using Question.Data;
using Relic;
using Relic.Data;
using Reward.Data;
using Sirenix.OdinInspector;
using Stage;
using Random = UnityEngine.Random;

namespace Reward
{
    public class RewardManager : MonoBehaviour
    {
        [Required, InlineEditor] public DeckData commonCardDeck;
        [Required, InlineEditor] public ItemDropData itemDropData;
        [Required, InlineEditor] public RewardContainerData rewardContainerData;

        [Required] public QuestionStoneDropTable questionStoneDropTable;
        
        // 單例存取方式
        public static RewardManager Instance  => GameManager.Instance != null ? GameManager.Instance.RewardManager : null;

        /// <summary>
        /// 根據獎勵類型取得對應的圖片
        /// </summary>
        public Sprite GetRewardSprite(RewardType rewardType)
        {
            return rewardContainerData.RewardsSprites[rewardType];
        }

        /// <summary>
        /// 根據機率決定獲得角色卡或通用卡（戰鬥勝利後）
        /// 並確保抽到的卡牌不重複
        /// </summary>
        public List<CardData> GetCombatWinCardList(int amount)
        {
            var cards = new List<CardData>();
            var drawnCardSet = new HashSet<CardData>(); // 用於檢查是否已抽過

            int attempts = 0;
            int maxAttempts = amount * 10; // 避免死循環

            while (cards.Count < amount && attempts < maxAttempts)
            {
                attempts++;

                var gainType = Random.Range(0f, 1f) < itemDropData.commonCardDropRate
                    ? ItemGainType.Common
                    : ItemGainType.Character;

                var rewardData = new RewardData { ItemGainType = gainType };
                var card = GetCard(rewardData);

                if (card != null && !drawnCardSet.Contains(card))
                {
                    drawnCardSet.Add(card);
                    cards.Add(card);
                }
            }

            if (cards.Count < amount)
            {
                Debug.LogWarning($"[RewardManager] 僅抽出 {cards.Count}/{amount} 張不重複卡牌，可能卡池不足。");
            }

            return cards;
        }
        /// <summary>
        /// 根據獎勵資料取得一張卡牌
        /// </summary>
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
                    Debug.LogError("Unknown reward type");
                    return null;
            }
        }

        /// <summary>
        /// 計算金幣獎勵（考慮難度與樓層加成）
        /// </summary>
        public int GetMoney(RewardData rewardData, NodeType nodeType)
        {
            int baseCoin = rewardData.CoinGainType == CoinGainType.Specify
                ? rewardData.specifyCoin
                : itemDropData.GetNodeDropMoney(nodeType);

            float difficultyRate = GameManager.Instance.GetDifficultyMoneyDropRate();
            float layerRate = itemDropData.GetLayerMoneyRate(MapManager.Instance.currentMapIndex);

            return Mathf.FloorToInt(baseCoin * difficultyRate * layerRate);
        }

        /// <summary>
        /// 計算石頭獎勵（目前無加成倍率）
        /// </summary>
        public int GetStone(RewardData rewardData, NodeType nodeType)
        {
            int baseStone = rewardData.CoinGainType == CoinGainType.Specify
                ? rewardData.specifyCoin
                : itemDropData.GetNodeDropStone(nodeType);

            return Mathf.FloorToInt(baseStone * 1f); // 保留擴展空間
        }

        /// <summary>
        /// 計算答題獎勵
        /// </summary>
        public int GetQuestionReward(AnswerRecord record)
        {
            var stoneDropAmountsForQuestionCount = questionStoneDropTable.GetStoneDropAmountsForQuestionCount(record.QuestionCount);
            var rewardStone = stoneDropAmountsForQuestionCount[record.CorrectCount];

            return rewardStone;
        }

        /// <summary>
        /// 根據節點與獎勵資料取得對應遺物
        /// </summary>
        public RelicInfo GetRelic(NodeType nodeType, RewardData rewardData)
        {
            var relicName = rewardData.ItemGainType == ItemGainType.Specify
                ? rewardData.specifyRelic
                : itemDropData.GetRelicData(nodeType);

            return GameManager.Instance.RelicManager.GetRelicInfo(relicName);
        }
    }
}
