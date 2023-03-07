using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Power
{
    public class DexterityPower : PowerBase
    {
        public override PowerType PowerType => PowerType.Vulnerable;

        public DexterityPower()
        {
            CanNegativeStack = true;
        }
        
        public override float ModifyBlock(float blockAmount) {
            
            return  (blockAmount + Value) > 0 ?  (blockAmount + Value) : 0;
        }
    }
}