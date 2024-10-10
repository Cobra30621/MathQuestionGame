using GameListener;
using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 狂怒的增傷
    /// </summary>
    public class FrenzyHelperPower : PowerBase
    {
        public override PowerName PowerName => PowerName.FrenzyHelper;

        public FrenzyHelperPower()
        {
            CanNegativeStack = true;
            DamageCalculateOrder = CalculateOrder.AdditionAndSubtraction;
        }
        
        
        public override float AtDamageGive(float damage)
        {
            // 造成傷害傳進來
            return damage + Amount;
        }
    }
}