using Combat;
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
                if (CombatManager.Instance.MainAlly.HasPower(PowerName.Equip))
                {
                    
                }
                else
                {
                    Owner.ClearPower(PowerName, GetEffectSource());
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