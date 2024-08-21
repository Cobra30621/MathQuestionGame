using GameListener;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
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
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }

        public override float AtDamageReceive(float damage)
        {
            return damage * 0;
        }
    }
}