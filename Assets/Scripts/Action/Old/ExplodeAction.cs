// using System;
// using Card;
// using NueGames.Enums;
// using NueGames.Action;
// using Card.Data;
// using Action.Parameters;
//
// namespace Action.Enemy
// {
//     public class ExplodeAction : GameActionBase
//     {
//         // 創立無用牌並改成他的ID
//         private string addCardId = "1";
//         private PileType _pileType = (PileType) 0;
//         private int _damage = 10;
//         
//         public ExplodeAction(SkillInfo skillInfo)
//         {
//             SkillInfo = skillInfo;
//         }
//         protected override void DoMainAction()
//         {
//             CardData cardData;
//             var find = CardManager.Instance.GetCardDataWithId(addCardId, out cardData);
//             CollectionManager.RemoveCardFromPile(_pileType, cardData);
//             var damageInfo = new DamageInfo(_damage, ActionSource);
//             var damageAction = new DamageAction(damageInfo, TargetList);
//             damageAction.DoAction();
//         }
//     }
// }