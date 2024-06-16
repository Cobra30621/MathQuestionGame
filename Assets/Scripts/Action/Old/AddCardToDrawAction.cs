// using System.Collections.Generic;
// using Action.Parameters;
// using Card;
// using Card.Data;
// using NueGames.Data.Collection;
// using NueGames.Enums;
// using UnityEngine;
//
// namespace NueGames.Action
// {
//     /// <summary>
//     /// 新增卡牌至牌組
//     /// </summary>
//     public class AddCardToDrawAction : GameActionBase
//     {
//         private List<CardData> _cards;
//         
//         public AddCardToDrawAction(List<string> cardIds, ActionSource source)
//         {
//             _cards = cards;
//             ActionSource = source;
//         }
//
//         public override void SetEffectInfo(SkillInfo skillInfo)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         protected override void DoMainAction()
//         {
//             foreach (var uiCard in _cards)
//             {
//                 CollectionManager.AddCardToPile(PileType.Draw, 
//                     uiCard);
//             }
//         }
//     }
// }