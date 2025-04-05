using GameListener;

namespace Power.Buff
{
    /// <summary>
    /// 無敵，本回合不會受到傷害
    /// </summary>
    public class InvinciblePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Invincible;

        public InvinciblePower()
        {
            DecreaseOnTurnStart = true;
            DamageCalculateOrder = CalculateOrder.FinalChange;
        }
        
        
        /// <summary>
        /// 能力清除時，更新敵人狀態
        /// </summary>
        public override void DoOnPowerClear()
        {
            UpdateEnemyIntentionDisplay();
        }
        
        /// <summary>
        /// 能力改變時，更新敵人狀態
        /// </summary>
        /// <param name="amount"></param>
        public override void DoOnPowerChanged(int amount)
        {
            UpdateEnemyIntentionDisplay();
        }


        public override float AtDamageReceive(float damage)
        {
            return 0;
        }
    }
}