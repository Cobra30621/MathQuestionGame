using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 給予傷害
    /// </summary>
    public class DamageAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.Damage;
        
   
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            if (IsTargetNull()) return;
            
            PlayFx(FxType.Attack, Target.transform);
            PlaySpawnTextFx($"{DamageInfo.GetAfterBlockDamage()}", Target);
            
            Target.BeAttacked(DamageInfo);
        }
    }
}