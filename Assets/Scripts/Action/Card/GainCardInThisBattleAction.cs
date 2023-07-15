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

        protected override void DoMainAction()
        {
            if (CollectionManager != null)
                CollectionManager.AddCardToPile(ActionData.TargetPile, ActionData.TargetCardData);
            else
                Debug.LogError("There is no CollectionManager");
            
        }
    }
}