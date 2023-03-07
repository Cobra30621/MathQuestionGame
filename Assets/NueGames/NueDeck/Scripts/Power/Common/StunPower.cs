using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
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