using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Characters;
using Power;

namespace NueGames.Action
{
    public class MultiBlockAction : GameActionBase
    {
        /// <summary>
        /// 護盾值
        /// </summary>
        private int _applyValue;
        /// <summary>
        /// 施放次數
        /// </summary>
        private int _times;
        
        
        public MultiBlockAction(int applyValue, int times)
        {
            _applyValue = applyValue;
            _times = times;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public MultiBlockAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _applyValue = skillInfo.EffectParameterList[0];
            _times = skillInfo.EffectParameterList[1];
        }
        
        protected override void DoMainAction()
        {
            for(int i = 0; i < _times; i++)
            {
                foreach (var target in TargetList)
                {
                    target.ApplyPower(PowerName.Block, _applyValue);
                }
            }
            
        }
    }
}