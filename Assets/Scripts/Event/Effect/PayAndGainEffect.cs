using System.Collections.Generic;
using Managers;
using Map;
using Money;

namespace NueGames.Event.Effect
{
    public class PayAndGainEffect : IEffect
    {
        
        private PayAndGainData data;
        
        public void Init(EffectData effectData)
        {
            data = effectData.PayAndGainData;
        }

        public void Execute(System.Action onComplete)
        {
            PayNeedCost();
            
            UIManager.Instance.RewardCanvas.ShowReward
                (data.RewardData, NodeType.Event, false, onComplete);
        }


        private void PayNeedCost()
        {
            switch (data.PaymentType)
            {
                case PaymentType.Health:
                    GameManager.Instance.AllyHealthHandler.AddHealth(-data.needHealth);
                    break;
                case PaymentType.Money:
                    GameManager.Instance.CoinManager.
                        Buy(new Dictionary<CoinType, int>(){
                            {CoinType.Money, data.needMoney}
                        });
                    break;
            }
        }
        

        public bool IsSelectable()
        {
            return GameManager.Instance.CoinManager.EnoughCoin(new Dictionary<CoinType, int>()
            {
                { CoinType.Money, data.needMoney }
            });
        }
    }
}