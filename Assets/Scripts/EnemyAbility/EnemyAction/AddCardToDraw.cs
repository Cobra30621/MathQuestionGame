// using System.Collections.Generic;
// using Card.Data;
// using NueGames.Action;
// using NueGames.Data.Collection;
// using NueGames.Managers;
//
// namespace EnemyAbility.EnemyAction
// {
//     /// <summary>
//     /// 新增卡牌至牌組
//     /// </summary>
//     public class AddCardToDraw : EnemyActionBase
//     {
//         public override int DamageValueForIntention => -1;
//         public override bool IsDamageAction => false;
//
//         public List<CardData> addCards;
//         
//         protected override void DoMainAction()
//         {
//             GameActionExecutor.AddToBottom(new AddCardToDrawAction(addCards, GetActionSource()));
//         }
//     }
// }