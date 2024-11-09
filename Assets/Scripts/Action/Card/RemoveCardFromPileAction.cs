using System;
using Card;
using Card.Data;
using NueGames.Enums;
using NueGames.Collection;
namespace NueGames.Action
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