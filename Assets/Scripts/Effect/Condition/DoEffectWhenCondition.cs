using System.Collections.Generic;
using Effect.Card;
using Effect.Condition;
using Effect.Parameters;
using UnityEngine;

namespace Effect.Other
{
    public class DoEffectWhenCondition : EffectBase
    {

        private JudgeCondition _condition;
        
        private EffectName doEffectName;

        private List<int> doEffectParameters;

        private SkillInfo _skillInfo;
        
        #region 建構值
        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public DoEffectWhenCondition(SkillInfo skillInfo)
        {
            _skillInfo = skillInfo;
            _condition = (JudgeCondition) skillInfo.EffectParameterList[0];
            doEffectName = (EffectName) skillInfo.EffectParameterList[1];
            
            doEffectParameters = new List<int>();
            for (int i = 2; i < skillInfo.EffectParameterList.Count; i++)
            {
                doEffectParameters.Add(skillInfo.EffectParameterList[i]);
            }
        }
        
        #endregion
        
        public override void Play()
        {
            Debug.Log($"Check Condition {_condition}");
            // 如果符合條件，執行效果
            if (ConditionChecker.PassCondition(_condition))
            {
                Debug.Log($"Pass Condition {_condition}");
                _skillInfo.EffectParameterList = doEffectParameters;
                _skillInfo.EffectID = doEffectName;
                var doEffect = EffectFactory.GetEffect(_skillInfo, TargetList, EffectSource);
                EffectExecutor.ExecuteImmediately(doEffect);
            }
        }
    }
}