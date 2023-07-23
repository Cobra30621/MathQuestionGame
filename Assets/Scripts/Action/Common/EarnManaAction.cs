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
        public override ActionName ActionName => ActionName.EarnMana;
   
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (CombatManager != null)
                CombatManager.AddMana(ActionData.BaseValue);
            else
                Debug.LogError("There is no CombatManager");
        }
    }
}