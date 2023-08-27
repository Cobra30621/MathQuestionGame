using NueGames.Characters;
using UnityEngine; 

namespace EnemyAbility
{
    // An abstract class serving as a base for various conditions
    public abstract class ConditionBase
    {
        protected EnemyBase enemy; // A reference to the EnemyBase class for this condition
        
        // Method to set the enemy for this condition
        public void SetEnemy(EnemyBase enemyBase)
        {
            Debug.Log("SetEnemy");
            enemy = enemyBase;
        }
        
        /// <summary>
        /// This method is used to evaluate a condition
        /// </summary>
        /// <returns></returns>
        public abstract bool Judge(); 
    }
    
    // An enumeration representing different judge conditions
    public enum JudgeCondition
    {
        GreaterThan,      // When a value is greater than another
        LessThan,         // When a value is less than another
        EqualTo,          // When a value is equal to another
        GreaterOrEqual,   // When a value is greater than or equal to another
        LessOrEqual       // When a value is less than or equal to another
    }
}