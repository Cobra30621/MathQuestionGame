using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NueGames.Characters;
using NueGames.Data.Collection;
using UnityEngine;

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
        /// 加成數量
        /// </summary>
        public float MultiplierAmount;
        
        /// <summary>
        /// 卡片資料(卡牌行為才需要)
        /// </summary>
        // TODO: 移除或是合併到 ActionData
        public CardData CardData;
        

        #endregion

        
        /// <summary>
        /// 加乘後的數值
        /// </summary>
        public int AdditionValue =>
            Mathf.RoundToInt(ActionData.BaseValue + 
                             MultiplierAmount * ActionData.MultiplierValue);

        
        public ActionParameters()
        {
            ActionData = new ActionData();
            ActionSource = new ActionSource();
            TargetList = new List<CharacterBase>();
            MultiplierAmount = 0;
        }


        public override string ToString()
        {
            return $"{nameof(ActionData)}: {ActionData}\n" +
                   $"{nameof(TargetList)}: {TargetList.Aggregate("", (current, character) => current + character.name + ", ")}\n" +
                   $"{nameof(ActionSource)}: {ActionSource}\n" +
                   $"{nameof(MultiplierAmount)}: {MultiplierAmount}";
        }
    }
}