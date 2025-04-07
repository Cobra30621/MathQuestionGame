using Combat.Card;
using GameListener;
using UnityEngine;

namespace Power.Mana
{
    /// <summary>
    /// 本局對戰中，所有的卡牌消耗變為1
    /// </summary>
    public class ManaSpringsPower : PowerBase
    {
        public override PowerName PowerName => PowerName.ManaSprings;

        public ManaSpringsPower()
        {
            CardManaCalculateOrder = CalculateOrder.DirectChange;


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