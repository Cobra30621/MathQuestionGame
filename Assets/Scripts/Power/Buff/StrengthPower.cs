using Characters;
using GameListener;
using NueGames.Enums;

namespace Power.Buff
{
    /// <summary>
    /// 力量
    /// </summary>
    public class StrengthPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Strength;

        public StrengthPower()
        {
            CanNegativeStack = true;
            DamageCalculateOrder = CalculateOrder.AdditionAndSubtraction;
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


        public override float AtDamageGive(float damage)
        {
            return damage + Amount;
        }
    }
}