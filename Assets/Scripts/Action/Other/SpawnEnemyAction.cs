using System.Collections.Generic;
using EnemyAbility;
using NueGames.Action;

namespace Action.Other
{
    public class SpawnEnemyAction : GameActionBase
    {
        private readonly List<EnemyData> _enemyDataList;

        public SpawnEnemyAction(List<EnemyData> enemyDataList)
        {
            _enemyDataList = enemyDataList;
        }

        protected override void DoMainAction()
        {
            foreach (var enemyData in _enemyDataList)
            {
                CombatManager.BuildEnemy(enemyData);
            }
        }
    }
}