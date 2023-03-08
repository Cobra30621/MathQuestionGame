using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
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