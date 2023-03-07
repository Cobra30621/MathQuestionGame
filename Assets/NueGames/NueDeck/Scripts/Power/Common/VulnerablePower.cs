using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
{
    public class VulnerablePower : PowerBase
    {
        public override PowerType PowerType => PowerType.Vulnerable;

        public VulnerablePower()
        {
            DecreaseOverTurn = true;
        }

        public override float AtDamageReceive(float damage)
        {
            return damage * 1.5f;
        }
    }
}