using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 獲得瑪娜
    /// </summary>
    public class EarnManaAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.EarnMana;
        public EarnManaAction()
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
            
            if (CombatManager != null)
                CombatManager.IncreaseMana(baseValue);
            else
                Debug.LogError("There is no CombatManager");

            PlayFx();
            PlayAudio();
        }
    }
}