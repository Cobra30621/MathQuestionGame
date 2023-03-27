using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Power
{
    public class StrengthPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Strength;

        public StrengthPower()
        {
            CanNegativeStack = true;
        }
        
        protected override void OnAttacked(DamageInfo info, int damageAmount)
        {
            Debug.Log($"{Owner} 攻擊力增加{Value}");
        }
    }
}