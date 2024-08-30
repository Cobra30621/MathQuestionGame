using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 賦予能力
    /// </summary>
    public class ApplyPowerAction : GameActionBase
    {
        /// <summary>
        /// 狀態層數
        /// </summary>
        private int _applyValue;
        /// <summary>
        /// 給予狀態
        /// </summary>
        private PowerName _targetPower;

        /// <summary>
        /// 內部系統用
        /// </summary>
        /// <param name="applyValue"></param>
        /// <param name="powerName"></param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>
        public ApplyPowerAction(int applyValue, PowerName powerName, 
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            _applyValue = applyValue;
            _targetPower = powerName;
            TargetList = targetList;
            ActionSource = actionSource;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public ApplyPowerAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _applyValue = skillInfo.EffectParameterList[1];
            _targetPower =  PowerHelper.GetPowerName(skillInfo.EffectParameterList[0]);
        }

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.ApplyPower(_targetPower, _applyValue);
            }
            
        }
    }
}