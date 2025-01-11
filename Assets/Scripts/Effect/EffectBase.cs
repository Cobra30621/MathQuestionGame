using System.Collections.Generic;
using Characters;
using Combat;
using Combat.Card;
using Effect.Parameters;
using Feedback;
using UnityEngine;

namespace Effect
{
    /// <summary>
    /// 用來實作遊戲效果（ex: 給予傷害、抽牌、回血）的基底 class
    /// 可以供其他系統（ex: 卡牌、遺物、敵人、道具等等）
    /// </summary>
    public abstract class EffectBase
    {
        #region Parameters
        /// <summary>
        /// 行為目標對象
        /// </summary>
        public List<CharacterBase> TargetList { get; protected set; }
        
        /// <summary>
        /// 行為來源
        /// </summary>
        public ActionSource ActionSource  { get; protected set; }


        /// <summary>
        /// 取得傷害基礎值，針對傷害類型的效果。
        /// 回傳一個包含兩個整數的 tuple，分別為傷害基礎值和傷害次數。
        /// </summary>
        /// <returns>
        /// 包含兩個整數的 tuple，分別為傷害基礎值和傷害次數。
        /// 預設值為 (-1, 1)。
        /// </returns>
        public virtual (int, int) GetDamageBasicInfo()
        {
            return (-1, 1);
        }
        
        /// <summary>
        /// 戰鬥管理器
        /// </summary>
        protected CombatManager CombatManager => CombatManager.Instance;

        /// <summary>
        /// 卡組管理器
        /// </summary>
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        
        #endregion
        
        
        #region 設定基礎數值 (For 讀表)

        public void SetBasicValue(List<CharacterBase> targets, ActionSource actionSource)
        {
            TargetList = targets;
            ActionSource = actionSource;
        }
        
        #endregion
        
        #region 執行遊戲行為

        /// <summary>
        /// 執行遊戲行為
        /// </summary>
        public void DoAction()
        {
            // 執行遊戲主要邏輯
            DoMainAction(); 
        }
        
        
        /// <summary>
        /// 執行遊戲的主要邏輯
        /// </summary>
        protected abstract void DoMainAction();
        

        #endregion
        
        
        /// <summary>
        /// 生成文字特效(如收到傷害顯示傷害數值)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="spawmRoot"></param>
        protected void PlaySpawnTextFx(string info, Transform spawmRoot)
        {
            if (spawmRoot == null)
                return;
           
            FxManager.Instance.SpawnFloatingText(spawmRoot,info);
        }
    }
}