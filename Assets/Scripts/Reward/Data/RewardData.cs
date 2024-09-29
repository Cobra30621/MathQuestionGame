using System;
using Card.Data;
using NueGames.Relic;
using Sirenix.OdinInspector;

namespace Reward
{
    [Serializable]
    public class RewardData
    {
        [LabelText("獎勵類型")] public RewardType RewardType;

        [LabelText("獲取方式")] 
        [ShowIf("IsShowItemGainType")]
        public ItemGainType ItemGainType;

        [LabelText("貨幣獲取方式")] 
        [ShowIf("IsShowCoinGainType")]
        public CoinGainType CoinGainType;

        
        [LabelText("指定的卡牌")] 
        [ShowIf("IsShowSpecifyCard")]
        public CardData specifyCard;

        [LabelText("指定的貨幣量")] 
        [ShowIf("IsShowSpecifyCoin")]
        public int specifyCoin;

        [LabelText("指定的遺物")] 
        [ShowIf("IsShowSpecifyRelic")]
        public RelicName specifyRelic;

        private bool IsShowItemGainType()
        {
            return RewardType == RewardType.Relic || RewardType == RewardType.Card;
        }


        private bool IsShowCoinGainType()
        {
            return RewardType == RewardType.Money || RewardType == RewardType.Stone;
        }

        private bool IsShowSpecifyCard()
        {
            return RewardType == RewardType.Card && ItemGainType == ItemGainType.Specify;
        }

        private bool IsShowSpecifyCoin()
        {
            return (RewardType == RewardType.Money || RewardType == RewardType.Stone) && 
                   CoinGainType == CoinGainType.Specify;
        }

        private bool IsShowSpecifyRelic()
        {
            return RewardType == RewardType.Relic && ItemGainType == ItemGainType.Specify;
        }
    }
}