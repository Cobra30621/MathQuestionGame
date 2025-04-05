using Combat.Card;
using GameListener;

namespace Power.Mana
{
    public class ManaSavingPower : PowerBase
    {
        public override PowerName PowerName => PowerName.ManaSaving;
        
        public ManaSavingPower()
        {
            CardManaCalculateOrder = CalculateOrder.FinalChange;
            
            
        }

        public override void Init()
        {
            CollectionManager.Instance.UpdateAllCardsManaCost();
        }


        public override void OnUseCard(BattleCard card)
        {
            base.OnUseCard(card);
            ClearPower();
            CollectionManager.Instance.UpdateAllCardsManaCost();
        }

        public override float GetCardRawMana(float rawValue)
        {
            return 0;
        }
    }
}