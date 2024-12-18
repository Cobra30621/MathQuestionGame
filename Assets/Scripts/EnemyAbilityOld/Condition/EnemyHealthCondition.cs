// using System.Security.Cryptography.X509Certificates;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace EnemyAbilityOld.Condition
// {
//     public class EnemyHealthCondition : ConditionBase
//     {
//         
//         [SerializeField] private JudgeCondition condition;
//         
//         [Range(0f,1f)]
//         [SerializeField] private float healthPercentage;
//         
//         public override ConditionBase GetCopy()
//         {
//             return new EnemyHealthCondition()
//             {
//                 condition = condition,
//                 healthPercentage = healthPercentage,
//                 enemy = enemy
//             };
//         }
//
//         public override bool Judge()
//         {
//             Debug.Log($"Enemy {enemy}");
//             float variableToCompare = enemy.GetHealth();
//             float valueToCompare = healthPercentage * enemy.GetMaxHealth();
//             
//             switch (condition)
//             {
//                 case JudgeCondition.GreaterThan:
//                     return variableToCompare > valueToCompare;
//                 case JudgeCondition.LessThan:
//                     return variableToCompare < valueToCompare;
//                 case JudgeCondition.EqualTo:
//                     return variableToCompare == valueToCompare;
//                 case JudgeCondition.GreaterOrEqual:
//                     return variableToCompare >= valueToCompare;
//                 case JudgeCondition.LessOrEqual:
//                     return variableToCompare <= valueToCompare;
//                 default:
//                     return false;
//             }
//         }
//     }
// }