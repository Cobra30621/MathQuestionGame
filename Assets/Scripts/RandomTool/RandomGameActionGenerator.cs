using NueGames.Action;
using NueGames.Card;
using NueGames.Data.Collection;
using UnityEngine;

namespace RandomTool
{
    public static class RandomGameActionGenerator
    {
        public static GameActionBase GetRandomAction(RandomActionData randomActionData, ActionParameters parameters)
        {
            float totalProbability = 0;
            foreach (var action in randomActionData.randomActionList)
            {
                totalProbability += action.Probability;
            }

            float randomValue = Random.Range(0f, totalProbability);
            float cumulativeProbability = 0;

            foreach (var action in randomActionData.randomActionList)
            {
                cumulativeProbability += action.Probability;
                if (cumulativeProbability >= randomValue)
                {
                    ActionData actionData = action.ActionData;
                    ActionParameters newParameters = new ActionParameters()
                    {
                        ActionType = actionData.GameActionType,
                        Value = actionData.ActionValue,
                        Self = parameters.Self,
                        Target = parameters.Target,
                        ActionSource = parameters.ActionSource, 
                        ActionData = actionData,
                        CardData = parameters.CardData
                    };
                    
                    GameActionBase gameAction = GameActionGenerator.GetGameAction(newParameters);
                    
                    Debug.Log($"randomValue : {randomValue}, Do Action {gameAction}");
                    
                    return gameAction;
                }
            }

            // 如果所有事件概率之和小于等于 0，则返回 null
            Debug.LogError("所有事件概率之和小于等于 0");
            return null;
        }
    }
}