using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.Power;
using UnityEngine;
using UnityEngine.Rendering;

namespace NueGames.Action
{
    /// <summary>
    /// 用來實作遊戲行為（ex: 給予傷害、抽牌、回血）的基底 class
    /// 可以供其他系統（ex: 卡牌、遺物、敵人、道具等等）
    /// </summary>
    public abstract class GameActionBase
    {
        #region Parameters
        /// <summary>
        /// 行為目標對象
        /// </summary>
        protected List<CharacterBase> TargetList;
        
        /// <summary>
        /// 行為來源
        /// </summary>
        public ActionSource ActionSource;
        
        public float ActionDelay = 0.1f;
        
        public SkillInfo SkillInfo { get; protected set; }
        
        #endregion
        
        #region Manager

        protected FxManager FxManager => FxManager.Instance;

        protected CombatManager CombatManager => CombatManager.Instance;

        protected CollectionManager CollectionManager => CollectionManager.Instance;


        #endregion


        #region SetValue

        public void SetBasicValue(List<CharacterBase> targets, ActionSource actionSource, float delay = 0.1f)
        {
            TargetList = targets;
            ActionSource = actionSource;
            ActionDelay = delay;
        }
        

        public void SetDelay(float delay)
        {
            ActionDelay = delay;
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
        
        
        #region Play FX

        /// <summary>
        /// 生成文字特效(如收到傷害顯示傷害數值)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="spawmRoot"></param>
        protected void PlaySpawnTextFx(string info, Transform spawmRoot)
        {
           
            FxManager.SpawnFloatingText(spawmRoot,info);
        }
        

        #endregion


        public override string ToString()
        {
            return $"{nameof(TargetList)}: {TargetList}, {nameof(ActionSource)}: {ActionSource}, {nameof(ActionDelay)}: {ActionDelay}";
        }
    }
}