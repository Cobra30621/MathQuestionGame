using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 在戰鬥中獲得卡片
    /// </summary>
    public class GainCardInThisBattleAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.GainCardInThisBattle;
        public override void DoAction()
        {
            if (CollectionManager != null)
                CollectionManager.AddCardToPile(TargetPile, TargetCardData);
            else
                Debug.LogError("There is no CollectionManager");
            
        }
    }
}