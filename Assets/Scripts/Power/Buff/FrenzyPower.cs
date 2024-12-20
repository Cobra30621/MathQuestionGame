using System.Collections.Generic;
using Action.Parameters;
using Combat;
using Enemy;
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
    public class FrenzyPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Frenzy;
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
            var originalDamage = info.DamageValue;
            int stackAmount = Mathf.CeilToInt(originalDamage * 0.5f);
      
            if (source != null)
            {
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(stackAmount, PowerName.Strength, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
                // 觸發後減層數 1 
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
            }
        }
    }
}