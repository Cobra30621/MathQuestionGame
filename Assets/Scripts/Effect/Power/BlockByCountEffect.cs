using Power;
using UnityEngine;

namespace Effect.Power
{
    /// <summary>
    /// 根據[1]對目標造成[2]點護盾
    /// </summary>
    public class BlockByCountEffect : EffectBase
    {
        private int count;
        private int type;
        private int baseBlock;
        private int block;
        public BlockByCountEffect(SkillInfo skillInfo)
        {
            type = skillInfo.EffectParameterList[0];
            baseBlock = skillInfo.EffectParameterList[1];
        }

        public override void Play()
        {
            switch(type)
            {
                case 1:
                    count = CombatManager.EnemyCount;
                    break;
                case 2:
                    count = CollectionManager.UsedCardCount;
                    break;
                default:
                    Debug.LogError("Invalid type");
                    break;
            }
            block = count * baseBlock;
            foreach (var target in TargetList)
            {
                target.ApplyPower(PowerName.Block, block, EffectSource);
            }
        }
    }
}