using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    public class WeakPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Weak;

        public WeakPower()
        {
            DecreaseOverTurn = true;
        }
        
        public override float AtDamageGive(float damage)
        {
            return damage * 0.75f;
        }
    }
}