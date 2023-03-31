using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 暈眩，無法行動
    /// </summary>
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