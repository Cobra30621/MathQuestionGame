using System.Collections.Generic;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    public class DamageAllEnemyAction : GameActionBase
    {
        private DamageInfo damageInfo;
        
        public DamageAllEnemyAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(ActionParameters parameters)
        {
            ActionData data = parameters.ActionData;
            Duration = parameters.ActionData.ActionDelay;
            
            SetValue(new DamageInfo(parameters.Value, parameters.SelfCharacter),
                parameters.TargetCharacter);
        }
        
        public void SetValue(DamageInfo info, CharacterBase target)
        {
            damageInfo = info;
            Target = target;

            HasSetValue = true;
        }
        
        public override void DoAction()
        {
            CheckHasSetValue();
            
            List<EnemyBase> enemyCopy = new List<EnemyBase>(CombatManager.CurrentEnemiesList);
            foreach (EnemyBase enemy in enemyCopy)
            {
                int value = CombatCalculator.GetDamageValue(damageInfo.Value, damageInfo.SelfCharacter, enemy);
                enemy.CharacterStats.Damage(value);
                PlayFx();
                PlaySpawnTextFx($"{value}");
            }
            
            PlayAudio();
        }
    }
}