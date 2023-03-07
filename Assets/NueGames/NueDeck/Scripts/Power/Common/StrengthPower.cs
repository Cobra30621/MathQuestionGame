using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
{
    public class StrengthPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Strength;

        public StrengthPower()
        {
            CanNegativeStack = true;
        }
        
        public override float AtDamageGive(float damage)
        {
            return damage + Value;
        }
    }
}