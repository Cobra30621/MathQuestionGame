using Power;
using UnityEngine;

namespace Effect.Power
{
    
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
        protected override void DoMainAction()
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
            Debug.Log("count: " + count);
            block = count * baseBlock;
            foreach (var target in TargetList)
            {
                target.ApplyPower(PowerName.Block, block);
            }
        }
    }
}