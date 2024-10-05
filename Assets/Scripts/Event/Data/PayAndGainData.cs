using System;
using System.Collections.Generic;
using Reward;
using Sirenix.OdinInspector;

namespace NueGames.Event
{
    [Serializable]
    public class PayAndGainData
    {
        [LabelText("支付東西")]
        public PaymentType PaymentType;
        
        [LabelText("所需血量")]
        public int needHealth;
        
        [LabelText("所需金錢")]
        public int needMoney;
        
        [LabelText("獲得獎勵")]
        public List<RewardData> RewardData;
    }

    public enum PaymentType
    {
        [LabelText("血量")]
        Health,
        [LabelText("金錢")]
        Money,
    }
}