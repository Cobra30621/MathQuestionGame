using Effect.Damage;
using Effect.Parameters;
using Power;

namespace Effect.Condition
{
    /// <summary>
    /// 如果本回合使用過 [1] 張以上的卡片，獲得 [2] 點格檔
    /// </summary>
    
    /// CAUTION: This class may not usable now!!!
    public class AddBlockWhenUseEnoughCardEffect : EffectBase
    {
        private int _requireCardCount;
        private int _blockAmount;
        
        public AddBlockWhenUseEnoughCardEffect(int requireCardCount, int blockAmount)
        {
            _requireCardCount = requireCardCount;
            _blockAmount = blockAmount;
        }

        public AddBlockWhenUseEnoughCardEffect(SkillInfo skillInfo)
        {
            _requireCardCount = skillInfo.EffectParameterList[0];
            _blockAmount = skillInfo.EffectParameterList[1];
        }
        
        
        public override void Play()
        {
            if (ConditionChecker.UseEnoughCard(_requireCardCount))
            {
                foreach (var target in TargetList)
                {
                    target.ApplyPower(PowerName.Block, _blockAmount, EffectSource);
                }
            }
            
        }
    }
}