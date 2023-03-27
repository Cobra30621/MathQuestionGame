using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    public class StunPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Stun;
        
        public StunPower()
        {
            DecreaseOverTurn = true;
        }
        
        // TODO: 處發條件
    }
}