using NueGames.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyAbility
{
    /// <summary>
    /// Represents Skill for an enemy.
    /// </summary>
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "Enemy/Enemy Data", order = 1)]
    public class EnemyData : SerializedScriptableObject
    {
        /// <summary>
        /// The maximum health of the enemy.
        /// </summary>
        public int MaxHealth;

        /// <summary>
        /// The abilities of the enemy.
        /// </summary>
        public EnemyAbilityData EnemyAbilityData;

        /// <summary>
        /// The prefab representing the enemy in the game.
        /// </summary>
        public EnemyBase EnemyPrefab;
    }
}