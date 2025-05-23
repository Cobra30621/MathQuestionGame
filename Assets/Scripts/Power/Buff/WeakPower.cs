﻿using GameListener;

namespace Power.Buff
{
    /// <summary>
    /// 虛弱，造成的傷害變 0.5 倍
    /// </summary>
    public class WeakPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Weak;

        public WeakPower()
        {
            DecreaseOnTurnEnd = true;
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

        
        public override float AtDamageGive(float damage)
        {
            return damage * 0.5f;
        }
    }
}