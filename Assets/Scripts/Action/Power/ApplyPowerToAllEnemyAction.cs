using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 賦予所有敵人能力
    /// </summary>
    public class ApplyPowerToAllEnemyAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.ApplyPowerToAllEnemy;
        

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (EnemyBase enemy in CombatManager.CurrentEnemiesList)
            {
                enemy.CharacterStats.ApplyPower(ActionData.PowerType, AdditionValue);
                PlayFx(FxName.Buff, enemy.transform);
            }
        }
    }
}