using Combat;
using Effect.Card;
using UnityEngine;

namespace Effect.Condition
{
    public static class ConditionChecker
    {
        public static bool PassCondition(JudgeCondition condition)
        {
            switch (condition)
            {
                case JudgeCondition.FirstCardInThisTurn:
                    return CombatManager.Instance.CombatCounter.IsFirstUseCardInCurrentTurn();
                case JudgeCondition.NotHurtEnemyInThisTurn:
                    return CombatManager.Instance.CombatCounter.IsNoEnemyHurtInCurrentTurn();
                default:
                    Debug.LogError($"Can't find condition {condition}");
                    return true;
            }

            return true;
        }

        public static bool UseEnoughCard(int needCard)
        {
            return needCard <= CombatManager.Instance.CombatCounter.UseCardCountInCurrentTurn;
        }

    }
}