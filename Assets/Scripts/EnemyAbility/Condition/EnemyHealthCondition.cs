using System.Security.Cryptography.X509Certificates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyAbility
{
    public class EnemyHealthCondition : ConditionBase
    {
        
        [SerializeField] private JudgeCondition condition;
        
        [Range(0f,1f)]
        [SerializeField] private float healthPercentage;
        
        private float variableToCompare;
        private float valueToCompare;
        
        public override bool Judge()
        {
            Debug.Log($"Enemy {enemy}");
            variableToCompare = enemy.GetHealth();
            valueToCompare = healthPercentage * enemy.GetMaxHealth();
            
            switch (condition)
            {
                case JudgeCondition.GreaterThan:
                    return variableToCompare > valueToCompare;
                case JudgeCondition.LessThan:
                    return variableToCompare < valueToCompare;
                case JudgeCondition.EqualTo:
                    return variableToCompare == valueToCompare;
                case JudgeCondition.GreaterOrEqual:
                    return variableToCompare >= valueToCompare;
                case JudgeCondition.LessOrEqual:
                    return variableToCompare <= valueToCompare;
                default:
                    return false;
            }
        }
    }
}