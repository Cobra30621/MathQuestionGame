using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 增加最大生命值
    /// </summary>
    public class IncreaseMaxHealthAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.IncreaseMaxHealth;

        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            if (IsTargetNull()) return;
            
            Target.CharacterStats.IncreaseMaxHealth(AdditionValue);

            PlayFx(FxType.Buff, Target.transform);
            PlaySpawnTextFx(AdditionValue.ToString(), Target);
        }
    }
}