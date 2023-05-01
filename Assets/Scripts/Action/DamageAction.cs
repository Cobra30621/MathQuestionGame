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
        
        public DamageAction()
        {
            FxType = FxType.FeedBackTest;
            AudioActionType = AudioActionType.Attack;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            if (IsTargetNull()) return;
            
            Target.BeAttacked(damageInfo);

            PlayFx();
            PlaySpawnTextFx($"{DamageValue}", Target);
            PlayAudio();
        }
    }
}