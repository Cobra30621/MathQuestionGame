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
    public static class EffectFactory
    {

        public static List<EffectBase> GetEffects(List<SkillInfo> skillInfos, List<CharacterBase> targets, 
            ActionSource actionSource)
        {
            var gameActions = new List<EffectBase>();
            foreach (var effectInfo in skillInfos)
            {
                var gameAction = GetEffect(effectInfo, targets, actionSource);
                gameActions.Add(gameAction);
                
            }

            return gameActions;
        }
        
        public static EffectBase GetEffect(SkillInfo skillInfo, List<CharacterBase> specifiedTargets, 
            ActionSource actionSource)
        {
            EffectBase action = BuildEffect(skillInfo);

            var targets = GetTargets(skillInfo.Target, specifiedTargets);
            
            action.SetBasicValue(targets, actionSource);
            return action;
        }


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
        

        private static EffectBase BuildEffect(SkillInfo skillInfo)
        {
            switch (skillInfo.EffectID)
            {
                case EffectName.Damage:
                    return new DamageEffect(skillInfo);
                case EffectName.MultiDamage:
                    return new MultiDamageEffect(skillInfo);
                case EffectName.ApplyPower:
                    return new ApplyPowerEffect(skillInfo);
                case EffectName.EnemyHpDamage:
                    return new EnemyHpDamageEffect(skillInfo);
                case EffectName.MultiBlock:
                    return new MultiBlockEffect(skillInfo);
                case EffectName.GainMana:
                    return new GainManaEffect(skillInfo);
                case EffectName.SpawnEnemy:
                    return new SpawnEnemyEffect(skillInfo);
                case EffectName.SplitEnemy:
                    return new SplitEnemyEffect(skillInfo);
                case EffectName.DemonicSacrifice:
                    return new DemonicSacrificeEffect(skillInfo);
                case EffectName.AddCardToPile:
                    return new AddCardToPileEffect(skillInfo);
                case EffectName.RemoveCardFromPile:
                    return new RemoveCardFromPileEffect(skillInfo);
                case EffectName.DamageByCount:
                    return new DamageByCountEffect(skillInfo);
                case EffectName.BlockByCount:
                    return new BlockByCountEffect(skillInfo);
                default:
                    Debug.LogError($"无效的技能类型 {skillInfo.EffectID}, 來自 {skillInfo.SkillID}");
                    return new NullEffect(skillInfo);
            }
        }
    }
}