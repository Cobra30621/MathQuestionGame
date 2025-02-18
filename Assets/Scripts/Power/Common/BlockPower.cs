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

        
        
        public override void UpdateStatusOnTurnStart()
        {
            // 如果沒有裝甲，回合結束清除格檔
            if (!Owner.HasPower(PowerName.Equip))
            {
                Owner.ClearPower(PowerName, GetEffectSource());
            }
        }
        

        public override void StackPower(int rawAmount)
        {
            int stackAmount = CombatCalculator.GetBlockValue(rawAmount, Owner);
            base.StackPower(stackAmount);
        }
    }
}