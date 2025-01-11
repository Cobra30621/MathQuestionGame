using System.Collections.Generic;
using Action;
using Action.Parameters;
using Action.Power;
using Characters;
using UnityEngine;

namespace Power.Buff
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
                GameActionExecutor.ExecuteImmediately(
                    new ApplyPowerAction(stackAmount, PowerName.Strength, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
                // 觸發後減層數 1 
                GameActionExecutor.ExecuteImmediately(
                    new ApplyPowerAction(-1, PowerName, 
                        new List<CharacterBase>(){Owner}, GetActionSource()));
            }
        }
    }
}