using System.Collections.Generic;
using Action.Parameters;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Parameters;

namespace CardAction
{
    public abstract class CardActionBase
    {
       
        
        protected CardBase Card;
        protected CardData CardData;
        protected List<CharacterBase> TargetList;

        public void SetValue(CardBase cardBase, List<CharacterBase> targetList)
        {
            Card = cardBase;
            CardData = cardBase.CardData;
            TargetList = targetList;
        }
        
        public abstract void DoAction();

        public ActionSource GetActionSource()
        {
            return new ActionSource()
            {
                SourceType = SourceType.Card,
                SourceCard = Card,
                SourceCharacter = CombatManager.Instance.CurrentMainAlly
            };
        }
    }
}