// using System.Collections.Generic;
// using Action.Parameters;
// using Card;
// using NueGames.Card;
// using NueGames.Characters;
// using NueGames.Combat;
// using NueGames.Data.Collection;
// using NueGames.Enums;
// using NueGames.Managers;
// using NueGames.Power;
//
// namespace NueGames.Action
// {
//     /// <summary>
//     /// 根據玩家的能力層數，給予傷害
//     /// </summary>
//     public class DamageByAllyPowerValueAction : GameActionBase
//     {
//         /// <summary>
//         /// 目標的能力
//         /// </summary>
//         private PowerName _targetPower;
//         /// <summary>
//         /// 加成數值
//         /// </summary>
//         private float _multiplierAmount;
//
//
//         public DamageByAllyPowerValueAction()
//         {
//         }
//
//         /// <param name="targetList"></param>
//         /// <param name="targetPower"></param>
//         /// <param name="source"></param>
//         /// <param name="multiplierAmount">給予能力 x 倍的傷害</param>
//         /// <param name="fixDamage"></param>
//         /// <param name="canPierceArmor"></param>
//         public DamageByAllyPowerValueAction(PowerName targetPower, 
//             int multiplierAmount = 1, bool fixDamage  = false, bool canPierceArmor  = false)
//         {
//             _targetPower = targetPower;
//             _multiplierAmount = multiplierAmount;
//             
//             
//         }
//
//         public override void SetEffectInfo(SkillInfo skillInfo)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         /// <summary>
//         /// 執行遊戲行為的功能
//         /// </summary>
//         protected override void DoMainAction()
//         {
//             float damageValue = CombatManager.MainAlly.GetPowerValue(_targetPower) * _multiplierAmount;
//     
//             var damageInfo = new DamageInfo(damageValue, ActionSource);
//             var damageAction = new DamageAction(damageInfo, TargetList, ActionSource, 0.5f);
//          
//             GameActionExecutor.AddAction(damageAction);
//         }
//     }
// }