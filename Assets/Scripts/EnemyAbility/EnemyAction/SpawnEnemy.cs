// using System.Collections.Generic;
// using Action.Other;
// using NueGames.Managers;
//
// namespace EnemyAbility.EnemyAction
// {
//     /// <summary>
//     /// 生成敵人
//     /// </summary>
//     public class SpawnEnemy : EnemyActionBase
//     {
//         public override int DamageValueForIntention => -1;
//         public override bool IsDamageAction => false;
//
//         public List<EnemyData> EnemyDataList;
//         protected override void DoMainAction()
//         {
//             GameActionExecutor.AddToBottom(new SpawnEnemyAction(EnemyDataList));
//         }
//     }
// }