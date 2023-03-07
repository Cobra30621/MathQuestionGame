using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
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
            Debug.Log(stackAmount);
            if (IsActive)
            {
                Value += stackAmount;
            }
            else
            {
                Value = stackAmount;
                IsActive = true;
            }
        }
    }
}