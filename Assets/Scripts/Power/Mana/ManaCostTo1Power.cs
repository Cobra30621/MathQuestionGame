using Combat.Card;
using GameListener;
using UnityEngine;

namespace Power.Mana
{
    /// <summary>
    /// 本局對戰中，所有的卡牌消耗變為1
    /// </summary>
    public class ManaCostTo1Power : PowerBase
    {
        public override PowerName PowerName => PowerName.ManaCostTo1;

        public ManaCostTo1Power()
        {
            CardManaCalculateOrder = CalculateOrder.FinalChange;
            
            
        }

        public override void Init()
        {
            CollectionManager.Instance.UpdateAllCardsManaCost();
        }


        public override float GetCardRawMana(float rawValue)
        {
            return 1;
        }
    }
}