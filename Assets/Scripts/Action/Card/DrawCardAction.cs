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
            BaseValue = drawCard;
            HasSetValue = true;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(BaseValue);
            else
                Debug.LogError("There is no CollectionManager");
            
            // PlayFx();
            // PlayAudio();
        }
    }
}