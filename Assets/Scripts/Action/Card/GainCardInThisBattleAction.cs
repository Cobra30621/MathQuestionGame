using Action.Parameters;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 在戰鬥中獲得卡片
    /// </summary>
    public class GainCardInThisBattleAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.GainCardInThisBattle;

        public GainCardInThisBattleAction(int cardCount, CardTransfer cardTransfer, ActionSource actionSource)
        {
            SetCardTransferActionValue(cardCount, cardTransfer, actionSource);
        }
        
        
        protected override void DoMainAction()
        {
            if (CollectionManager != null)
                CollectionManager.AddCardToPile(ActionData.CardTransfer.TargetPile, 
                    ActionData.CardTransfer.TargetCardData);
            else
                Debug.LogError("There is no CollectionManager");
            
        }
    }
}