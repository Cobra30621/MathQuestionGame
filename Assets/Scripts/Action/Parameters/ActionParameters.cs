using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Collection;

namespace Action.Parameters
{
    /// <summary>
    /// 遊戲行為(GameAction)所需參數
    /// </summary>
    public class ActionParameters
    {
        #region 必填

        /// <summary>
        /// 行為目標對象
        /// </summary>
        public List<CharacterBase> TargetList;

        /// <summary>
        /// 行為來源
        /// </summary>
        public ActionSource ActionSource;
        /// <summary>
        /// 行為資料
        /// </summary>
        public ActionData ActionData;
        

        #endregion


        #region 選填

        /// <summary>
        /// 卡片資料(卡牌行為才需要)
        /// </summary>
        // TODO: 移除或是合併到 ActionData
        public CardData CardData;
        

        #endregion

        public ActionParameters()
        {
        }

    }
}