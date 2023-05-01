using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 產生遊戲行為(GameAction)
    /// </summary>
    public static class GameActionGenerator
    {
        private static Dictionary<string, Type> _gameActionDict = new Dictionary<string, Type>();

        static GameActionGenerator()
        {
            SetUpGameActionClasses();
        }

        private static void SetUpGameActionClasses()
        {
            IEnumerable<Type> gameActionClasses = Assembly.GetAssembly(typeof(GameActionBase)).GetTypes()
                .Where(t => typeof(GameActionBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (Type gameActionClass in gameActionClasses)
            {
                _gameActionDict.Add(gameActionClass.Name, gameActionClass);
            }
        }

        /// <summary>
        /// 產生遊戲行為(GameAction)
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="actionSource"></param>
        /// <param name="actionDataList"></param>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<GameActionBase> GetGameActions(CardData cardData, ActionSource actionSource,
            List<ActionData> actionDataList, CharacterBase self,
            CharacterBase target)
        {
            List<GameActionBase> gameActionBases = new List<GameActionBase>();
            foreach (var actionData in actionDataList)
            {
                ActionParameters actionParameters = new ActionParameters()
                {
                    ActionType = actionData.GameActionType,
                    Value = actionData.ActionValue,
                    Self = self,
                    Target = target,
                    ActionSource = actionSource, 
                    ActionData = actionData,
                    CardData = cardData
                };

                GameActionBase gameActionBase = GetGameAction(actionParameters);
                gameActionBases.Add(gameActionBase);
            }

            return gameActionBases;
        }

        /// <summary>
        /// 產生遊戲行為(GameAction)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static GameActionBase GetGameAction(ActionParameters parameters)
        {
            GameActionBase gameActionBase = GetGameAction(parameters.ActionType);
            gameActionBase.SetValue(parameters);
            return gameActionBase;
        }

        /// <summary>
        /// 產生遊戲行為(GameAction)
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        private static GameActionBase GetGameAction(GameActionType actionType)
        {
            string gameActionName = actionType.ToString() + "Action";

            if (_gameActionDict.ContainsKey(gameActionName))
            {
                return Activator.CreateInstance(_gameActionDict[gameActionName]) as GameActionBase;
            }
            
            Debug.LogError($"沒有 GameAction {gameActionName} 的 Class");
            return null;
        }
    }
}