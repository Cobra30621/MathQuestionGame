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
        public override PowerType PowerType => PowerType.Strength;

        public StrengthPower()
        {
            CanNegativeStack = true;
        }
        
        
        public override float AtDamageGive(float damage)
        {
            return damage + Amount;
        }
    }
}