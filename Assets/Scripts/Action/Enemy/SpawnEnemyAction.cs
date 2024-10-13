using Card;
using NueGames.Action;

namespace Action.Enemy
{
    /// <summary>
    /// This class represents an action to spawn enemies in the game.
    /// It inherits from GameActionBase and is responsible for handling the logic of spawning enemies.
    /// </summary>
    public class SpawnEnemyAction : GameActionBase
    {
        private readonly int _spawnCount;
        private readonly string _spawnEnemyId;

        /// <summary>
        /// Constructor for the SpawnEnemyAction class.
        /// Initializes the spawn count and enemy ID based on the provided skill information.
        /// </summary>
        /// <param name="skillInfo">The skill information containing the necessary parameters for spawning enemies.</param>
        public SpawnEnemyAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _spawnCount = skillInfo.EffectParameterList[1];
            _spawnEnemyId = $"{skillInfo.EffectParameterList[0]}";
        }

        /// <summary>
        /// Performs the main action of spawning enemies.
        /// Accesses the character handler to build the specified number of enemies with the given ID.
        /// </summary>
        protected override void DoMainAction()
        {
            var characterHandler = CombatManager.characterHandler;

            for (int i = 0; i < _spawnCount; i++)
            {
                characterHandler.BuildEnemy(_spawnEnemyId);
            }
        }
    }
}