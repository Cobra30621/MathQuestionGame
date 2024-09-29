using System;
using Card.Data;
using NueGames.Relic;
using Sirenix.OdinInspector;

namespace Reward
{
    [Serializable]
    public class RewardData
    {
        [LabelText("獎勵類型")]
        public RewardType RewardType;

        [LabelText("卡片或遺物獲取方式")]
        public ItemGainType ItemGainType;

        [LabelText("貨幣獲取方式")]
        public CoinGainType CoinGainType;

        [LabelText("指定的卡牌")] public CardData specifyCard;
        
        [LabelText("指定的貨幣量")] public int specifyCoin;

        [LabelText("指定的遺物")] public RelicName specifyRelic;
    }
}