using System.Collections.Generic;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 對所有敵人造成傷害
    /// </summary>
    public class DamageAllEnemyAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageAllEnemy;
        
        public DamageAllEnemyAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            
            List<EnemyBase> enemyCopy = new List<EnemyBase>(CombatManager.CurrentEnemiesList);
            foreach (EnemyBase enemy in enemyCopy)
            {
                damageInfo.Target = enemy;
                enemy.BeAttacked(damageInfo);
                
                PlaySpawnTextFx($"{DamageValue}", enemy);
            }
            
            PlayFx();
            PlayAudio();
        }
    }
}