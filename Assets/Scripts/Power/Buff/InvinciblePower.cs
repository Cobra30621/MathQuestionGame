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
            DecreaseOverTurn = true;
            DamageCalculateOrder = CalculateOrder.FinalChange;
        }

        public override float AtDamageReceive(float damage)
        {
            return 0;
        }
    }
}