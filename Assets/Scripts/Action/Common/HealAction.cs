using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 回血
    /// </summary>
    public class HealAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.Heal;
  
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (IsTargetNull()) return;
            
            Target.CharacterStats.Heal(AdditionValue);

            PlayFx(FxType.Heal, Target.transform);
            PlaySpawnTextFx($"{AdditionValue}", Target);
        }
    }
}