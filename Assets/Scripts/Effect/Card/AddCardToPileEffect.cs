using Combat.Card;

namespace Effect.Card
{
    public class AddCardToPileEffect : EffectBase
    {
        private string addCardId;
        private PileType _pileType;
        
        public AddCardToPileEffect(SkillInfo skillInfo)
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