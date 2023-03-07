using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
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