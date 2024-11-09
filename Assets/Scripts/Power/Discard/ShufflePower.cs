// using System.Collections.Generic;
// using Action.Parameters;
// using Combat;
// using NueGames.Action;
//
// using NueGames.Managers;
//
// namespace NueGames.Power
// {
//
//
//     /// <summary>
//     /// 在回合結束時將隨機手牌洗入牌堆
//     /// </summary>
//     public class ShufflePower : PowerBase
//     {
//         public override PowerName PowerName => PowerName.Shuffle;
//         
//         
//         public override void SubscribeAllEvent()
//         {
//             CombatManager.OnTurnStart += OnTurnStart;
//         }
//
//         public override void UnSubscribeAllEvent()
//         {
//             CombatManager.OnTurnStart -= OnTurnStart;
//         }
//         
//         
//
//
//         protected override void OnTurnStart(TurnInfo info)
//         {
//             if (IsCharacterTurn(info))
//             {
//                 GameActionExecutor.AddAction(new RemoveCardFromPileAction(0));
//                 GameActionExecutor.AddAction(new RemoveCardFromPileAction(1));
//             }
//         }
//     }
// }