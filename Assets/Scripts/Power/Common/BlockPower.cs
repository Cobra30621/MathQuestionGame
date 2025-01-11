using Combat;
using NueGames.Combat;
using UnityEngine;

namespace Power.Common
{
    /// <summary>
    /// 格檔
    /// </summary>
    public class BlockPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Block;


        public override void SubscribeAllEvent()
        {
            Debug.Log($"Subscirbe Block {Owner.name}");
            CombatManager.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }


        protected override void OnTurnStart(TurnInfo info)
        {
            
            Debug.Log($"Character Turn:  {Owner.name} {info}" + IsCharacterTurn(info));
            if (IsCharacterTurn(info))
            {
                if (CombatManager.Instance.MainAlly.HasPower(PowerName.Equip))
                {
                    
                }
                else
                {
                    Owner.ClearPower(PowerName);
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