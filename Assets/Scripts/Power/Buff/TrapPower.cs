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
    public class TrapPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Trap;
        public override void SubscribeAllEvent()
        {
            Owner.OnAttacked += OnAttacked;
        }

        public override void UnSubscribeAllEvent()
        {
            Owner.OnAttacked -= OnAttacked;
        }
        protected override void OnAttacked(DamageInfo info)
        {
            var source = info.ActionSource.SourceCharacter;
            Debug.Log(source);
            if (source != null)
            {
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(2, PowerName.Weak, 
                        new List<CharacterBase>() {info.ActionSource.SourceCharacter}, GetActionSource()));
                // 觸發後減層數 1 
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
            }
        }
    }
}