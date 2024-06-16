// using System.Collections.Generic;
// using EnemyAbility;
// using NueGames.Action;
// using System;
// using System.Linq;
//
// namespace Action.Other
// {
//     public class HiveSpawnBeeAction : GameActionBase
//     {
//         private readonly List<EnemyData> _enemyDataList;
//
//         public HiveSpawnBeeAction(List<EnemyData> enemyDataList)
//         {
//             _enemyDataList = enemyDataList;
//         }
//
//         protected override void DoMainAction()
//         {
//             // Shuffle the list of enemy data
//             var random = new Random();
//             var shuffledList = _enemyDataList.OrderBy(x => random.Next()).ToList();
//
//             // Select the first two enemies in the shuffled list
//             var selectedEnemies = shuffledList.Take(2).ToList();
//
//             // Build the selected enemies
//             foreach (var enemyData in selectedEnemies)
//             {
//                 CombatManager.BuildEnemy(enemyData);
//             }
//         }
//     }
// }