// using Action.Parameters;
// using NueGames.Enums;
// using UnityEngine;
//
// namespace NueGames.Action
// {
//     /// <summary>
//     /// 在戰鬥中獲得卡片
//     /// </summary>
//     public class GainCardInThisBattleAction : GameActionBase
//     {
//         private CardTransfer _cardTransfer;
//
//         public GainCardInThisBattleAction(CardTransfer cardTransfer, ActionSource source)
//         {
//             _cardTransfer = cardTransfer;
//             ActionSource = source;
//         }
//         
//         
//         protected override void DoMainAction()
//         {
//             if (CollectionManager != null)
//                 CollectionManager.AddCardToPile(_cardTransfer.TargetPile, 
//                     _cardTransfer.TargetCardData);
//             else
//                 Debug.LogError("There is no CollectionManager");
//             
//         }
//     }
// }