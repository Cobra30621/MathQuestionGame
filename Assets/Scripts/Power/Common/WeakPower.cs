
using GameListener;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 虛弱，受到的傷害變 0.75 倍
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
            return damage * 0.75f;
        }
    }
}