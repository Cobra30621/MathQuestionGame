using System.Collections.Generic;
using Action.Parameters;
using Combat;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.UI;
using UnityEngine;

namespace NueGames.Power
{


    /// <summary>
    /// 反彈能力
    /// </summary>
    public class ThornsPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Thorns;


        public override void SubscribeAllEvent()
        {
            Owner.OnAttacked += OnAttacked;
            CombatManager.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            Owner.OnAttacked -= OnAttacked;
            CombatManager.OnTurnStart -= OnTurnStart;
        }


        // 怪物攻擊時，造成傷害後反彈傷害
        protected override void OnAttacked(DamageInfo info)
        {
            var source = info.ActionSource.SourceCharacter;
            // 怪物攻擊時，造成傷害後反彈傷害
            Debug.Log(source);
            if (source != null)
            {
                // 造成與層數相等的傷害
                var damageInfo = new DamageInfo(Amount, GetActionSource(), fixDamage: true);
                GameActionExecutor.ExecuteImmediately(new DamageAction(damageInfo, new List<CharacterBase>() {info.ActionSource.SourceCharacter}));
         
                // 反彈後減層數 1 
                GameActionExecutor.ExecuteImmediately(
                    new ApplyPowerAction(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
      
            }
        }

    }
}