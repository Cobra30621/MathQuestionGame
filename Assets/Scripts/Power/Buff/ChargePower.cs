using System.Collections.Generic;
using GameListener;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Managers;

namespace Power.Buff
{
    /// <summary>
    /// 力量
    /// </summary>
    public class ChargePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Charge;
        
        public ChargePower()
        {
            DamageCalculateOrder = CalculateOrder.MultiplyAndDivide;
        }
        
        
        public override float AtDamageGive(float damage)
        {
            GameActionExecutor.AddAction(
                new ApplyPowerAction(-1, PowerName, 
                    new List<CharacterBase>(){Owner}, GetActionSource()));
            return damage*2;
        }
    }
}