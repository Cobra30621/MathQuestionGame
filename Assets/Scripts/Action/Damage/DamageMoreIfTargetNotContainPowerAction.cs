using System.Collections.Generic;
using Action.Parameters;
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
        public override ActionName ActionName => ActionName.DamageMoreIfTargetNotContainPowerAction;
        private PowerName _judgePower;
        private int _extraDamage;

        /// <summary>
        /// 如果對象有 XX 能力，造成額外傷害
        /// </summary>
        /// <param name="baseValue">基礎傷害</param>
        /// <param name="judgePower">判斷是否持有的能力</param>
        /// <param name="extraDamage">額外加乘傷害</param>
        /// <param name="targetList">目標對象</param>
        /// <param name="actionSource">行為來源</param>
        /// <param name="fixDamage">是否為固定傷害</param>
        /// <param name="canPierceArmor">是否為穿甲傷害</param>
        public DamageMoreIfTargetNotContainPowerAction(
            int baseValue, PowerName judgePower, int extraDamage, List<CharacterBase> targetList,  
            ActionSource actionSource, bool fixDamage = false, bool canPierceArmor = false)
        {
            SetDamageActionValue(baseValue, targetList, actionSource, fixDamage, canPierceArmor);
            _judgePower = judgePower;
            _extraDamage = extraDamage;
        }


        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                int totalDamage = ActionData.BaseValue;
                if (!target.HasPower(_judgePower))
                {
                    totalDamage += _extraDamage;
                }
                
                GameActionExecutor.AddToBottom(new DamageAction(
                    totalDamage, new List<CharacterBase>(){target}, Parameters.ActionSource));
            }
        }
    }
}