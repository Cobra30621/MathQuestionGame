using Card.Data;
using Combat.Card;
using NueGames.Enums;

namespace Action.Card
{
    public class RemoveCardFromPileAction : GameActionBase
    {
        private string cardId;
        private int _index;
        private CardData _cardData;
        private PileType _pileType;
        
        public RemoveCardFromPileAction(int index)
        {
            _pileType = (PileType) 1;
            _index = index;
        }
        
        public RemoveCardFromPileAction(SkillInfo skillInfo)
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