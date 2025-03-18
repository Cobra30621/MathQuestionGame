using GameListener;

namespace Power.Buff
{
    /// <summary>
    /// 易傷，受到的傷害變 1.5 倍
    /// </summary>
    public class VulnerablePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Vulnerable;

        public VulnerablePower()
        {
            DecreaseOverTurn = true;
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
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
            return damage * 1.5f;
        }
    }
}