using System;
using System.Collections.Generic;
using Reward;
using Reward.Data;
using Sirenix.OdinInspector;

namespace MapEvent.Data
{
    [Serializable]
    public class PayAndGainData
    {
        [LabelText("支付東西")]
        public PaymentType PaymentType;
        
        [LabelText("所需血量")]
        [ShowIf("IsPayHealth")]
        public int needHealth;
        
        [LabelText("所需金錢")]
        [ShowIf("IsPayMoney")]
        public int needMoney;
        
        [LabelText("獲得獎勵")]
        public List<RewardData> RewardData;
        
        private bool IsPayHealth()
        {
            return PaymentType == PaymentType.Health;
        }
        
        private bool IsPayMoney()
        {
            return PaymentType == PaymentType.Money;
        }
    }

    public enum PaymentType
    {
        [LabelText("血量")]
        Health,
        [LabelText("金錢")]
        Money,
    }
}