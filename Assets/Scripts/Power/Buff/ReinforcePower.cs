using GameListener;
using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 強化
    /// </summary>
    public class ReinforcePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Reinforce;

        public ReinforcePower()
        {
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }
        
        
        public override float AtDamageGive(float damage)
        {
            return damage*2;
        }
        public override float ModifyBlock(float blockAmount)
        {
            return blockAmount*2;
        }
    }
}