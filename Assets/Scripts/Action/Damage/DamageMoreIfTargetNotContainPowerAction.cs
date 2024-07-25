using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Characters;
using NueGames.Managers;
using NueGames.Power;

namespace NueGames.Action
{
    /// <summary>
    /// 如果對象有 XX 能力，造成額外傷害
    /// </summary>
    public class DamageMoreIfTargetNotContainPowerAction : GameActionBase
    {
        /// <summary>
        /// 判斷的能力
        /// </summary>
        private PowerName _judgePower;
        /// <summary>
        /// 基礎傷害
        /// </summary>
        private int _basicValue = 0;
        /// <summary>
        /// 額外傷害
        /// </summary>
        private int _extraDamage;

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public DamageMoreIfTargetNotContainPowerAction(SkillInfo skillInfo)
        {
            _judgePower = PowerHelper.GetPowerName(skillInfo.EffectParameterList[1]);
            _basicValue = skillInfo.EffectParameterList[1];
            _extraDamage = skillInfo.EffectParameterList[2];
        }

        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                float totalDamage = _basicValue;
                if (!target.HasPower(_judgePower))
                {
                    totalDamage += _extraDamage;
                }

                var damageInfo = new DamageInfo(totalDamage, ActionSource);
                var damageAction = new DamageAction(damageInfo, new List<CharacterBase>(){target});
         
                GameActionExecutor.AddAction(damageAction);
            }
        }
    }
}