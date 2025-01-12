using System.Collections.Generic;
using Characters;
using Combat;
using Effect.Card;
using Effect.Common;
using Effect.Damage;
using Effect.Enemy;
using Effect.Parameters;
using Effect.Power;
using UnityEngine;

namespace Effect
{
    /// <summary>
    /// 用於產生戰鬥中的效果，主要給表單使用
    /// </summary>
    public static class EffectFactory
    {
        /// <summary>
        /// 取得效果清單
        /// </summary>
        /// <param name="skillInfos"></param>
        /// <param name="targets"></param>
        /// <param name="effectSource"></param>
        /// <returns></returns>
        public static List<EffectBase> GetEffects(List<SkillInfo> skillInfos, List<CharacterBase> targets, 
            EffectSource effectSource)
        {
            var effects = new List<EffectBase>();
            foreach (var effectInfo in skillInfos)
            {
                var effect = GetEffect(effectInfo, targets, effectSource);
                effects.Add(effect);
                
            }

            return effects;
        }
        
        /// <summary>
        /// 取得效果
        /// </summary>
        /// <param name="skillInfo"></param>
        /// <param name="specifiedTargets"></param>
        /// <param name="effectSource"></param>
        /// <returns></returns>
        public static EffectBase GetEffect(SkillInfo skillInfo, List<CharacterBase> specifiedTargets, 
            EffectSource effectSource)
        {
            EffectBase effect = BuildEffect(skillInfo);

            var targets = GetTargets(skillInfo.Target, specifiedTargets);
            
            effect.SetBasicValue(targets, effectSource);
            return effect;
        }

        /// <summary>
        /// 取得目標對象
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="specifiedTargets"></param>
        /// <returns></returns>
        public static List<CharacterBase> GetTargets(ActionTargetType targetType, List<CharacterBase> specifiedTargets)
        {
            List<CharacterBase> targetList = new List<CharacterBase>();
            switch (targetType)
            {
                case ActionTargetType.Ally:
                    targetList.Add(CombatManager.Instance.MainAlly);
                    break;
                case ActionTargetType.SpecifiedEnemy:
                    targetList = specifiedTargets;
                    break;
                case ActionTargetType.AllEnemies:
                    targetList.AddRange(CombatManager.Instance.Enemies);
                    break;
                case ActionTargetType.RandomEnemy:
                    targetList.Add(CombatManager.Instance.RandomEnemy);
                    break;
            }

            return targetList;
        }
        

        /// <summary>
        /// 創建效果
        /// </summary>
        /// <param name="skillInfo"></param>
        /// <returns></returns>
        private static EffectBase BuildEffect(SkillInfo skillInfo)
        {
            switch (skillInfo.EffectID)
            {
                case EffectName.Damage:
                    return new DamageEffect(skillInfo);
                case EffectName.ApplyBlock:
                    return new ApplyBlockEffect(skillInfo);
                case EffectName.ApplyPower:
                    return new ApplyPowerEffect(skillInfo);
                case EffectName.Heal:
                    return new HealEffect(skillInfo);
                case EffectName.AddCardToPile:
                    return new AddCardToPileEffect(skillInfo);
                case EffectName.EnemyHpDamage:
                    return new EnemyHpDamageEffect(skillInfo);
                case EffectName.GainMana:
                    return new GainManaEffect(skillInfo);
                case EffectName.BlockByCount:
                    return new BlockByCountEffect(skillInfo);
                case EffectName.DamageByCount:
                    return new DamageByCountEffect(skillInfo);
                case EffectName.DrawCard:
                    return new DrawCardEffect(skillInfo);
                case EffectName.ClearPower:
                    return new ClearPowerEffect(skillInfo);
                
                case EffectName.SpawnEnemy:
                    return new SpawnEnemyEffect(skillInfo);
                case EffectName.SplitEnemy:
                    return new SplitEnemyEffect(skillInfo);
                case EffectName.DemonicSacrifice:
                    return new DemonicSacrificeEffect(skillInfo);
                default:
                    Debug.LogError($"无效的技能类型 {skillInfo.EffectID}, 來自 {skillInfo.SkillID}");
                    return new NullEffect(skillInfo);
            }
        }
    }
}