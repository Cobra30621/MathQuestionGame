using System.Collections.Generic;
using Characters;
using Combat;
using Effect.Common;
using Effect.Parameters;
using Effect.Power;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;

namespace Power.Test
{
    public class TestPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Test;
        
        
        public override void OnBeAttacked(DamageInfo info)
        {
            Debug.Log("OnAttacked");
            // 敵人觸法
            if (Owner.IsCharacterType(CharacterType.Enemy))
            {
                List<CharacterBase> targets = CombatManager.Instance.EnemiesForTarget();
                
                var effect = new ApplyPowerEffect(2, 
                    PowerName.Strength, targets, GetEffectSource());
                effect.Play();
            }
            
        }
        
        public override void OnDead(DamageInfo info)
        {
            Debug.Log("OnDeath");
            
            // 敵人觸法
            if (Owner.IsCharacterType(CharacterType.Enemy))
            {
                List<CharacterBase> targets = CombatManager.Instance.EnemiesForTarget();
                
                var effect = new ApplyPowerEffect(2, 
                    PowerName.Block, targets, GetEffectSource());
                effect.Play();
            }
        }

        public override void OnAttack(DamageInfo info, List<CharacterBase> targets)
        {
            Debug.Log("OnAttack");
            
            var effect = new ApplyPowerEffect(1, 
                PowerName.Strength, new List<CharacterBase>(){Owner}, GetEffectSource());
            effect.Play();
        }

        public override void OnBattleWin(int roundNumber)
        {
            Debug.Log("OnBattleWin");

            var healEffect = new HealEffect(10, 
                new List<CharacterBase>() { Owner }, GetEffectSource());
            healEffect.Play();
        }
    }
}