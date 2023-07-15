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
        public override ActionName ActionName => ActionName.DamageAllEnemy;
        

        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            List<CharacterBase> enemyCopy = new List<CharacterBase>(TargetList);
            foreach (CharacterBase enemy in enemyCopy)
            {
                DamageInfo.Target = enemy;
                PlaySpawnTextFx($"{DamageInfo.GetAfterBlockDamage()}", enemy);
                
                enemy.BeAttacked(DamageInfo);
                
            }
        }
    }
}