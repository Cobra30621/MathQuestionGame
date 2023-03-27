using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    public class BlockPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Block;

        public BlockPower()
        {
            ClearAtNextTurn = true;
        }
        
        public override void StackPower(int rawAmount)
        {
            int stackAmount = CombatCalculator.GetBlockValue(rawAmount, Owner);
            base.StackPower(stackAmount);
        }
    }
}