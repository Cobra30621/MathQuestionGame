using System.Collections.Generic;
using Action;
using Action.Parameters;
using Card;
using Combat;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;

namespace GameAction
{
    public static class GameActionFactory
    {

        public static List<GameActionBase> GetGameActions(List<SkillInfo> skillInfos, List<CharacterBase> targets, 
            ActionSource actionSource)
        {
            var gameActions = new List<GameActionBase>();
            foreach (var effectInfo in skillInfos)
            {
                var gameAction = GetGameAction(effectInfo, targets, actionSource);
                gameActions.Add(gameAction);
                
            }

            return gameActions;
        }
        
        public static GameActionBase GetGameAction(SkillInfo skillInfo, List<CharacterBase> specifiedTargets, 
            ActionSource actionSource)
        {
            GameActionBase action = BuildAction(skillInfo);

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
        

        private static GameActionBase BuildAction(SkillInfo skillInfo)
        {
            switch (skillInfo.EffectID)
            {
                case GameActionType.Damage:
                    return new DamageAction(skillInfo);
                case GameActionType.MultiDamage:
                    return new MultiDamageAction(skillInfo);
                case GameActionType.ApplyPower:
                    return new ApplyPowerAction(skillInfo);
                case GameActionType.Block:
                    return new ApplyBlockAction(skillInfo);
                case GameActionType.MultiBlock:
                    return new MultiBlockAction(skillInfo);
                case GameActionType.GainMana:
                    return new GainManaAction(skillInfo);
                case GameActionType.Meteorite:
                    return new MeteoriteAction(skillInfo);
                case GameActionType.Flee:
                    return new FleeAction(skillInfo);
                case GameActionType.Tear:
                    return new TearAction(skillInfo);
                default:
                    Debug.LogError("无效的技能类型");
                    return new NullAction(skillInfo);
            }
        }
    }
}