using Card.Data;
using Combat.Card;

namespace Effect.Card
{
    public class RemoveCardFromPileEffect : EffectBase
    {
        private string cardId;
        private int _index;
        private CardData _cardData;
        private PileType _pileType;
        
        public RemoveCardFromPileEffect(int index)
        {
            _pileType = (PileType) 1;
            _index = index;
        }
        
        public RemoveCardFromPileEffect(SkillInfo skillInfo)
        {
            cardId = $"{skillInfo.EffectParameterList[0]}";
            _pileType = (PileType) skillInfo.EffectParameterList[1];
        }
        
        
        protected override void DoMainAction()
        { 
            CollectionManager.RemoveCardFromPile(_pileType, _cardData);
        }
    }
}