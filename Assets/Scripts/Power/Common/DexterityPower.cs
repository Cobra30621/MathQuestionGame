using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 敏捷，增加卡片獲得的格檔數量
    /// </summary>
    public class DexterityPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Dexterity;

        public DexterityPower()
        {
            CanNegativeStack = true;
        }
        
        public override float ModifyBlock(float blockAmount) {
            
            return  (blockAmount + Amount) > 0 ?  (blockAmount + Amount) : 0;
        }
    }
}