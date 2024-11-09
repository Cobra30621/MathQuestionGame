using System;
using Card;
using Card.Data;
using NueGames.Enums;
using NueGames.Action;

namespace Action.Enemy
{
    public class DisturbAction : GameActionBase
    {
        // 創立無用牌並改成他的ID
        private string addCardId = "1";
        private PileType _pileType = (PileType) 0;
        public DisturbAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
        }
        protected override void DoMainAction()
        {
            CollectionManager.AddCardToPile(_pileType, addCardId);
            
            // 抽到的回合結束時
            /*
            CardData cardData;
            var find = CardManager.Instance.GetCardDataWithId(addCardId, out cardData);
            CollectionManager.RemoveCardFromPile(_pileType, cardData);
            */
        }
    }
}