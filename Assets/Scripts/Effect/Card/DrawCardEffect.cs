using Effect.Parameters;
using UnityEngine;

namespace Effect.Card
{
    /// <summary>
    /// 抽卡
    /// </summary>
    public class DrawCardEffect : EffectBase
    {
        /// <summary>
        /// 抽卡張數
        /// </summary>
        private int _drawCardCount;

        /// <summary>
        /// 內部系統使用
        /// </summary>
        /// <param name="drawCardCount"></param>
        /// <param name="source"></param>
        public DrawCardEffect(int drawCardCount, EffectSource source)
        {
            _drawCardCount = drawCardCount;
            EffectSource = source;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public DrawCardEffect(SkillInfo skillInfo)
        {
            _drawCardCount = skillInfo.EffectParameterList[0];
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void Play()
        {
            if (CollectionManager != null)
                CollectionManager.DrawCards(_drawCardCount);
            else
                Debug.LogError("There is no CollectionManager");
        }
    }
}