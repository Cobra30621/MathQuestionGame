using Action.Parameters;
using UnityEngine;

namespace Action.Card
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
            _drawCardCount = skillInfo.EffectParameterList[0];
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