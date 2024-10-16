using GameListener;
using NueGames.Combat;
using NueGames.Enums;
using UnityEngine;
using System.Collections.Generic;
using Action.Parameters;
using Combat;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;

namespace NueGames.Power
{
    /// <summary>
    /// 強化
    /// </summary>
    public class ReinforceAfterDeadPower : PowerBase
    {
        public override PowerName PowerName => PowerName.ReinforceAfterDead;

        public ReinforceAfterDeadPower()
        {
            
        }
        
        
        protected override void OnDead(DamageInfo damageInfo)
        {
            List<CharacterBase> targets = new List<CharacterBase>();
            var allEnemy = CombatManager.Instance.Enemies;
            targets.AddRange(allEnemy);

            // 對全體敵方單位施加強化
            GameActionExecutor.AddAction(
                new ApplyPowerAction(1, PowerName.Reinforce, 
                    targets, GetActionSource()));
        }
    }
}