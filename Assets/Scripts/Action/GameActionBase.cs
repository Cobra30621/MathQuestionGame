using System.Collections.Generic;
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
        /// 遊戲行為的資料
        /// </summary>
        public ActionData ActionData;
        /// <summary>
        /// 行動參數
        /// </summary>
        protected ActionParameters ActionParameters;
        
        /// <summary>
        /// 行為目標對象
        /// </summary>
        protected List<CharacterBase> TargetList;
        /// <summary>
        /// 行為的發起者
        /// </summary>
        protected CharacterBase Self;
        
        /// <summary>
        /// 加成數量
        /// </summary>
        protected float MultiplierAmount;

        /// <summary>
        /// 加乘後的數值
        /// </summary>
        protected int AdditionValue
        {
            get { return Mathf.RoundToInt(ActionData.BaseValue + 
                                          MultiplierAmount * ActionData.MultiplierValue); }
        }
        
        /// <summary>
        /// 傷害類型
        /// </summary>
        protected DamageInfo DamageInfo;

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

        /// <summary>
        /// 依據參數設定行為數值
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void SetValue(ActionParameters parameters)
        {
            ActionParameters = parameters;
            ActionData =  parameters.ActionData;
            MultiplierAmount = 0;
            
            Self = parameters.Self;
            TargetList = parameters.TargetList;

            DamageInfo = new DamageInfo(parameters);

        }

        public virtual void SetValue(DamageInfo info)
        {
            DamageInfo = info;
            ActionData.BaseValue = DamageInfo.BaseValue;
            ActionData.MultiplierValue = DamageInfo.MultiplierValue;
            MultiplierAmount = DamageInfo.MultiplierAmount;

            TargetList = new List<CharacterBase>() { info.Target };

        }

        public virtual void SetValue(ApplyPowerParameters parameters)
        {
            ActionData.BaseValue = parameters.Value;
            TargetList = new List<CharacterBase>() {parameters.Target};
            ActionData.PowerType = parameters.PowerType;
            
        }

        #endregion

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

        
        /// <summary>
        /// 執行傷害行動
        /// </summary>
        protected void DoDamageAction()
        {
            DamageAction damageAction = new DamageAction();
            damageAction.SetValue(GetDamageInfoForDamageAction());
            GameActionExecutor.AddToBottom(damageAction);
        }

        /// <summary>
        /// 取得傷害行動用的 DamageInfo
        /// </summary>
        /// <returns></returns>
        protected DamageInfo GetDamageInfoForDamageAction()
        {
            Debug.Log("GetDamageInfoForDamageAction()");
            DamageInfo.BaseValue = ActionData.BaseValue;
            DamageInfo.MultiplierValue = ActionData.MultiplierValue;
            DamageInfo.MultiplierAmount = MultiplierAmount;
            return DamageInfo;
        }
        
    }
}