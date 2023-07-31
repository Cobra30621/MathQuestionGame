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
        private int _drawCardCount;

        public DrawCardAction(int drawCardCount, ActionSource source)
        {
            _drawCardCount = drawCardCount;
            ActionSource = source;
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(_drawCardCount);
            else
                Debug.LogError("There is no CollectionManager");
        }
    }
}