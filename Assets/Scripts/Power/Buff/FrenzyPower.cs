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
            Debug.Log(source);
            if (source != null)
            {
                // Apply一個類似力量的Power : 
                
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(2, PowerName.FrenzyHelper, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
                // 觸發後減層數 1 
                GameActionExecutor.AddAction(
                    new ApplyPowerAction(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
            }
        }
    }
}