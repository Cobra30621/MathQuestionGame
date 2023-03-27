using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
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