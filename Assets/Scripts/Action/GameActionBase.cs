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

        protected FxManager FxManager => FxManager.Instance;

        protected AudioManager AudioManager => AudioManager.Instance;

        protected GameManager GameManager => GameManager.Instance;

        protected CombatManager CombatManager => CombatManager.Instance;

        protected CollectionManager CollectionManager => CollectionManager.Instance;


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

        #region Damage

        protected void SetDamageActionValue(int baseValue, CharacterBase target,
            ActionSource actionSource, bool fixDamage = false, bool canPierceArmor = false)
        {
            SetDamageActionValue(baseValue,new List<CharacterBase>(){target}, actionSource, fixDamage, canPierceArmor);
        }

        protected void SetDamageActionValue(int baseValue, List<CharacterBase> targetList, 
            ActionSource actionSource, bool fixDamage  = false, bool canPierceArmor  = false)
        {
            ActionData.BaseValue = baseValue;
            ActionData.FixDamage = fixDamage;
            ActionData.CanPierceArmor = canPierceArmor;
            Parameters.ActionSource = actionSource;

            Parameters.TargetList = targetList;
        }
        

        #endregion

        #region Power

        protected void SetPowerActionValue(int baseValue, PowerName powerName,
            CharacterBase target, ActionSource actionSource)
        {
            SetPowerActionValue(baseValue, powerName, new List<CharacterBase>(){target}, actionSource);
        }

        protected void SetPowerActionValue(int baseValue, PowerName powerName, 
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            ActionData.BaseValue = baseValue;
            ActionData.powerName = powerName;
            Parameters.TargetList = targetList;
            Parameters.ActionSource = actionSource;
        }

        #endregion


        #region Card Transfer

        protected void SetCardTransferActionValue(int cardCount, CardTransfer cardTransfer, ActionSource actionSource)
        {
            ActionData.BaseValue = cardCount;
            ActionData.CardTransfer = cardTransfer;
            Parameters.ActionSource = actionSource;
        }

        #endregion
        
        #region Basic

        protected void SetBasicAndTargetValue(int baseValue, CharacterBase target, ActionSource actionSource)
        {
            SetBasicAndTargetValue(baseValue, new List<CharacterBase>(){target}, actionSource);
        }

        protected void SetBasicAndTargetValue(int baseValue, List<CharacterBase> targetList, ActionSource actionSource)
        {
            ActionData.BaseValue = baseValue;
            Parameters.TargetList = targetList;
            Parameters.ActionSource = actionSource;
        }

        protected void SetBasicValue(int baseValue, ActionSource actionSource)
        {
            ActionData.BaseValue = baseValue;
            Parameters.ActionSource = actionSource;
        }

        #endregion


        #region FX

        public void SetFXValue(FxName fxName,  FxSpawnPosition fxSpawnPosition)
        {
            Parameters.ActionData.FxName = fxName;
            Parameters.ActionData.FxSpawnPosition = fxSpawnPosition;
        }

        #endregion
        
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
                        FxManager.PlayFx(ActionData.FxName, spawnTransform, target.transform.position);
                    };
                    break;
                case FxSpawnPosition.Ally:
                    spawnTransform.position = CombatManager.GetMainAllyTransform().position;
                    FxManager.PlayFx(ActionData.FxName, spawnTransform);
                    break;
                case FxSpawnPosition.EnemyMiddle:
                case FxSpawnPosition.ScreenMiddle:
                    FxManager.PlayFx(ActionData.FxName, spawnTransform);
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
        

        #endregion


        #region 工具：執行傷害行為

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

        public override string ToString()
        {
            return $"{ActionName}\n{Parameters}";
        }
    }
}