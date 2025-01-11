using GameListener;

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
            DecreaseOverTurn = true;
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }
        
        public override float AtDamageGive(float damage)
        {
            return damage * 0.5f;
        }
    }
}