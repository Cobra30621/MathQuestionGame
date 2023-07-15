using System.Collections.Generic;
using Action.Parameters;
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
            List<CharacterBase> targetList = new List<CharacterBase>(TargetList);
            foreach (CharacterBase target in targetList)
            {
                DamageInfo damageInfo = CreateDamageInfo(target);
                PlaySpawnTextFx($"{damageInfo.GetAfterBlockDamage()}", target);
                
                target.BeAttacked(damageInfo);
                
            }
        }
    }
}