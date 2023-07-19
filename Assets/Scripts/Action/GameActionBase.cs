using System.Collections.Generic;
using Action.Parameters;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
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
        /// 行動類型 (用來確保一定會創立 GameActionType)
        /// </summary>
        public abstract ActionName ActionName  { get;}
        
        
        /// <summary>
        /// 行動參數
        /// </summary>
        protected ActionParameters Parameters;
        
        /// <summary>
        /// 行為目標對象
        /// </summary>
        protected List<CharacterBase> TargetList => Parameters.TargetList;
        /// <summary>
        /// 遊戲行為的資料
        /// </summary>
        public ActionData ActionData => Parameters.ActionData;



        /// <summary>
        /// 加乘後的數值
        /// </summary>
        protected int AdditionValue => Parameters.AdditionValue;

        #endregion
        
        #region Manager

        protected FxManager FxManager
        {
            get { return FxManager.Instance; }
        }

        protected AudioManager AudioManager
        {
            get { return AudioManager.Instance; }
        }

        protected GameManager GameManager
        {
            get { return GameManager.Instance; }
        }

        protected CombatManager CombatManager
        {
            get { return CombatManager.Instance; }
        }

        protected CollectionManager CollectionManager
        {
            get { return CollectionManager.Instance; }
        }

        protected GameActionExecutor GameActionExecutor
        {
            get { return GameActionExecutor.Instance; }
        }
        

        #endregion

        #region Set Value

        protected GameActionBase()
        {
            Parameters = new ActionParameters();
        }

        /// <summary>
        /// 依據參數設定行為數值
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void SetValue(ActionParameters parameters)
        {
            Parameters = parameters;
        }

        public virtual void SetDamageActionValue(int baseValue, List<CharacterBase> targetList, 
            ActionSource actionSource, bool fixDamage  = false, bool canPierceArmor  = false)
        {
            ActionData.BaseValue = baseValue;
            ActionData.FixDamage = fixDamage;
            ActionData.CanPierceArmor = canPierceArmor;
            Parameters.ActionSource = actionSource;

            Parameters.TargetList = targetList;
        }

        public virtual void SetPowerActionValue(int baseValue, PowerType powerType, 
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            ActionData.BaseValue = baseValue;
            ActionData.PowerType = powerType;
            Parameters.TargetList = targetList;
            Parameters.ActionSource = actionSource;
            
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
            // 執行特效撥放
            DoFXAction();
        }
        
        
        /// <summary>
        /// 執行遊戲的主要邏輯
        /// </summary>
        protected abstract void DoMainAction();
        

        #endregion
        #region Play FX

        /// <summary>
        /// 執行要撥放的特效
        /// </summary>
        protected void DoFXAction()
        {
            // 不播放特效
            if (ActionData.FxName == FxName.Null)
            {
                return;
            }

            var spawnTransform = FxManager.GetFXSpawnPosition(ActionData.FxSpawnPosition);

            switch (ActionData.FxSpawnPosition)
            {
                case FxSpawnPosition.EachTarget:
                    foreach (var target in TargetList)
                    {
                        spawnTransform.position = target.transform.position;
                        PlayFx(ActionData.FxName, spawnTransform);
                    };
                    break;
                case FxSpawnPosition.Ally:
                    spawnTransform.position = CombatManager.GetMainAllyTransform().position;
                    PlayFx(ActionData.FxName, spawnTransform);
                    break;
                case FxSpawnPosition.EnemyMiddle:
                case FxSpawnPosition.ScreenMiddle:
                    PlayFx(ActionData.FxName, spawnTransform);
                    break;
            }
        }
        
        /// <summary>
        /// 生成文字特效(如收到傷害顯示傷害數值)
        /// </summary>
        /// <param name="info"></param>
        protected void PlaySpawnTextFx(string info, CharacterBase target)
        {
          
            FxManager.SpawnFloatingText(target.TextSpawnRoot,info);
        }
        
        /// <summary>
        /// 播放特效
        /// </summary>
        protected void PlayFx(FxName fxName, Transform spawnPosition)
        {
            FxManager.PlayFx(spawnPosition, fxName);
        }

        #endregion


        #region 工具：執行傷害行為

        /// <summary>
        /// 執行傷害行動
        /// </summary>
        protected void DoDamageAction()
        {
            foreach (var target in TargetList)
            {
                DamageAction damageAction = new DamageAction();
                damageAction.SetValue(Parameters);
                GameActionExecutor.AddToBottom(damageAction);
            }
        }

        /// <summary>
        /// 取得傷害行動用的 DamageInfo
        /// </summary>
        /// <returns></returns>
        protected DamageInfo CreateDamageInfo(CharacterBase target)
        {
            DamageInfo damageInfo = new DamageInfo
            {
                Parameters = Parameters,
                Target = target
            };

            return damageInfo;
        }

        #endregion
        
    }
}