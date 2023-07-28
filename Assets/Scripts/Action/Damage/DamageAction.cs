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
        public override ActionName ActionName => ActionName.Damage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseValue">傷害數值</param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>
        /// <param name="fixDamage">固定傷害</param>
        /// <param name="canPierceArmor">可以突破護盾</param>
        public DamageAction(int baseValue, List<CharacterBase> targetList, 
            ActionSource actionSource, bool fixDamage  = false, bool canPierceArmor  = false)
        {
            SetDamageActionValue(baseValue, targetList, actionSource, fixDamage, canPierceArmor);
        }
   
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                DamageInfo damageInfo = CreateDamageInfo(target);
                
                PlaySpawnTextFx($"{damageInfo.GetDamageValue()}", target);
                target.BeAttacked(damageInfo);
            }
            
        }
    }
}