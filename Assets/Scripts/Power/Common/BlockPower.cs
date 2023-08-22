using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 格檔
    /// </summary>
    public class BlockPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Block;

        public BlockPower()
        {
            // ClearAtNextTurn = true;
        }

        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }


        protected override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                if (CombatManager.Instance.MainAlly.HasPower(PowerName.Solid))
                {
                    
                }
                else
                {
                    ClearPower();
                }
            }
        }

        public override void StackPower(int rawAmount)
        {
            int stackAmount = CombatCalculator.GetBlockValue(rawAmount, Owner);
            base.StackPower(stackAmount);
        }
    }
}