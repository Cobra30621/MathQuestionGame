using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;

using UnityEngine;
namespace NueGames.Action
{
    public class MeteoriteAction : GameActionBase
    {
        private int _damage = 100;
        public MeteoriteAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _damage = skillInfo.EffectParameterList[0];
        }
        protected override void DoMainAction()
        {
            if (CombatManager != null)
                CombatManager.SetMana(0);
            else
                Debug.LogError("There is no CombatManager");
            var damageInfo = new DamageInfo(_damage, ActionSource);
            var damageAction = new DamageAction(damageInfo, TargetList);
            damageAction.DoAction();
        }
    }
}