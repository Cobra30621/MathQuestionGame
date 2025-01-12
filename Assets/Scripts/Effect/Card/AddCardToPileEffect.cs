using Combat.Card;

namespace Effect.Card
{
    /// <summary>
    /// 加入卡牌 id [1] 的牌，進入 [2] 牌堆
    /// </summary>
    public class AddCardToPileEffect : EffectBase
    {
        /// <summary>
        /// 新增的卡牌 ID
        /// </summary>
        private string addCardId;
        
        /// <summary>
        /// 加入的卡組類型
        /// </summary>
        private PileType _pileType;
        
        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public AddCardToPileEffect(SkillInfo skillInfo)
        {
            addCardId = $"{skillInfo.EffectParameterList[0]}";
            _pileType = (PileType) skillInfo.EffectParameterList[1];
        }
        
        public override void Play()
        {
            CollectionManager.AddCardToPile(_pileType, addCardId);
            
        }
    }
}