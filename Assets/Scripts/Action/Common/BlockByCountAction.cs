using System.Collections.Generic;
using Card;
using NueGames.Combat;
using Action.Parameters;
using NueGames.Characters;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Action
{
    
    public class BlockByCountAction : GameActionBase
    {
        private int count;
        private int type;
        private int baseBlock;
        private int block;
        public BlockByCountAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
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