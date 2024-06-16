using Action.Parameters;
using Card;
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
        /// <summary>
        /// 抽卡張數
        /// </summary>
        private int _drawCardCount;

        public DrawCardAction(int drawCardCount, ActionSource source)
        {
            _drawCardCount = drawCardCount;
            ActionSource = source;
        }

        /// <summary>
        /// 讀表用
        /// </summary>
        /// <param name="skillInfo"></param>
        public DrawCardAction(SkillInfo skillInfo)
        {
            _drawCardCount = skillInfo.int1;
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