using Action.Parameters;
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
        public override ActionName ActionName => ActionName.DrawCard;
        
        public void SetValue(int cardCount, ActionSource source)
        {
            ActionData.BaseValue = cardCount;
            Parameters.ActionSource = source;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(ActionData.BaseValue);
            else
                Debug.LogError("There is no CollectionManager");
        }
    }
}