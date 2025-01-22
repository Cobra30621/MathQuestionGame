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


        public override void DoOnPowerClear()
        {
            UpdateEnemyIntentionDisplay();
        }
        
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