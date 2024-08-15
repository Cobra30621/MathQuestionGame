using GameListener;
using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
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
        
        
        public override float AtDamageGive(float damage)
        {
            return damage + Amount;
        }
    }
}