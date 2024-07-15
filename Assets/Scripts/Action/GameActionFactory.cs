using System.Collections.Generic;
using Action;
using Action.Parameters;
using Card;
using NueGames.Action;
using NueGames.Characters;
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
                var gameAction = GameActionFactory.GetGameAction(effectInfo, targets, actionSource);
                gameActions.Add(gameAction);
                
            }

            return gameActions;
        }
        
        public static GameActionBase GetGameAction(SkillInfo skillInfo, List<CharacterBase> targets, 
            ActionSource actionSource)
        {
            GameActionBase action = BuildAction(skillInfo);
            
            action.SetBasicValue(targets, actionSource);
            return action;
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
                default:
                    Debug.LogError("无效的技能类型");
                    return new NullAction(skillInfo);
            }
        }
    }
}