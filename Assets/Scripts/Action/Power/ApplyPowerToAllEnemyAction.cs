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
        public override GameActionType ActionType => GameActionType.ApplyPowerToAllEnemy;

        public ApplyPowerToAllEnemyAction()
        {
            FxType = FxType.Buff;
            AudioActionType = AudioActionType.Power;
        }
        

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            
            foreach (EnemyBase enemy in CombatManager.CurrentEnemiesList)
            {
                enemy.CharacterStats.ApplyPower(powerType, AdditionValue);
                PlayFx();
            }
            
            PlayAudio();
        }
    }
}