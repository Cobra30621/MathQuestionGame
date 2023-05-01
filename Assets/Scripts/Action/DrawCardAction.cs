using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 抽卡
    /// </summary>
    public class DrawCardAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.DrawCard;
        
        public void SetValue(int drawCard)
        {
            baseValue = drawCard;
            HasSetValue = true;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(baseValue);
            else
                Debug.LogError("There is no CollectionManager");
            
            // PlayFx();
            // PlayAudio();
        }
    }
}