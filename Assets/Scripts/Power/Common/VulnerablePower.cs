using GameListener;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 易傷，受到的傷害變 1.5 倍
    /// </summary>
    public class VulnerablePower : PowerBase
    {
        public override PowerType PowerType => PowerType.Vulnerable;

        public VulnerablePower()
        {
            DecreaseOverTurn = true;
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }

        public override float AtDamageReceive(float damage)
        {
            return damage * 1.5f;
        }
    }
}