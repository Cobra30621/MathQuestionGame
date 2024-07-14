// using NueGames.Action;
// using NueGames.Managers;
//
// namespace EnemyAbility.EnemyAction
// {
//     /// <summary>
//     /// Represents an enemy action that inflicts damage to targets.
//     /// </summary>
//     public class Damage : EnemyActionBase
//     {
//         public override int DamageValueForIntention => damageValue;
//         public override bool IsDamageAction => true;
//
//         /// <summary>
//         /// The value of damage to be inflicted.
//         /// </summary>
//         public int damageValue;
//
//         /// <summary>
//         /// Executes the main action of causing damage.
//         /// </summary>
//         protected override void DoMainAction()
//         {
//             GameActionExecutor.AddAction(new DamageAction(
//                 damageValue, TargetList, GetActionSource()));
//         }
//     }
// }