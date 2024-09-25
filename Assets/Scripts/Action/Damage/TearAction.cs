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
    public class TearAction : GameActionBase
    {
        private int _damage;
        public TearAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
        }
        protected override void DoMainAction()
        {
            _damage = 4 + 3 * CollectionManager.UsedCardCount;
            var damageInfo = new DamageInfo(_damage, ActionSource);
            var damageAction = new DamageAction(damageInfo, TargetList);
            damageAction.DoAction();
        }
    }
}