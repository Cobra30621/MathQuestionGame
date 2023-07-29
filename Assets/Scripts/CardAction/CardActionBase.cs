using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;

namespace CardAction
{
    public abstract class CardActionBase
    {
        protected CardBase Card;
        protected CardData CardData;
        protected List<CharacterBase> TargetList;

        protected FxManager FxManager => FxManager.Instance;
        
        public void SetValue(CardBase cardBase, List<CharacterBase> targetList)
        {
            Card = cardBase;
            CardData = cardBase.CardData;
            TargetList = targetList;
        }
        
        /// <summary>
        /// 執行遊戲行為
        /// </summary>
        public void DoAction()
        {
            // 執行遊戲主要邏輯
            DoMainAction(); 
            // 執行特效撥放
            DoFXAction();
        }

        
        /// <summary>
        /// 執行遊戲的主要邏輯
        /// </summary>
        protected abstract void DoMainAction();
        

        #region Play FX

        /// <summary>
        /// 執行要撥放的特效
        /// </summary>
        protected void DoFXAction()
        {
            GameActionExecutor.AddToBottom(new FXAction(
                CardData.FxName, CardData.FxSpawnPosition, TargetList));
        }
        
        #endregion
        
        protected ActionSource GetActionSource()
        {
            return new ActionSource()
            {
                SourceType = SourceType.Card,
                SourceCard = Card,
                SourceCharacter = CombatManager.Instance.CurrentMainAlly
            };
        }
    }
}