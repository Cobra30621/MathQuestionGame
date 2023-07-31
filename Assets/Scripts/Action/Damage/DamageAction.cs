using System.Collections.Generic;
using Action.Parameters;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 給予傷害
    /// </summary>
    public class DamageAction : GameActionBase
    {
        private DamageInfo _damageInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageValue">傷害數值</param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>
        /// <param name="fixDamage">固定傷害</param>
        /// <param name="canPierceArmor">可以突破護盾</param>
        public DamageAction(float damageValue, List<CharacterBase> targetList, 
            ActionSource actionSource, bool fixDamage  = false, bool canPierceArmor  = false)
        {
            TargetList = targetList;

            _damageInfo = new DamageInfo()
            {
                damageValue = damageValue,
                ActionSource = actionSource,
                FixDamage = fixDamage,
                CanPierceArmor = canPierceArmor
            };
        }


        public DamageAction(DamageInfo damageInfo, List<CharacterBase> targetList)
        {
            _damageInfo = damageInfo;
            TargetList = targetList;
        }
   
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                _damageInfo.Target = target;
                PlaySpawnTextFx($"{_damageInfo.GetDamageValue()}", target.TextSpawnRoot);
                target.BeAttacked(_damageInfo);
            }
            
        }
    }
}