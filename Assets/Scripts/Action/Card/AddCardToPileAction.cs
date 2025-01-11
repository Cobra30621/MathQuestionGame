using Combat.Card;
using NueGames.Enums;

namespace Action.Card
{
    public class AddCardToPileAction : GameActionBase
    {
        private string addCardId;
        private PileType _pileType;
        
        public AddCardToPileAction(SkillInfo skillInfo)
        {
            addCardId = $"{skillInfo.EffectParameterList[0]}";
            _pileType = (PileType) skillInfo.EffectParameterList[1];
        }
        
        
        protected override void DoMainAction()
        {
            CollectionManager.AddCardToPile(_pileType, addCardId);
            
        }
    }
}