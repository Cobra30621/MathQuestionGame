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
        /// <summary>
        /// 行動類型 (用來確保一定會創立 GameActionType)
        /// </summary>
        public abstract GameActionType ActionType  { get;}
        /// <summary>
        /// 行動參數
        /// </summary>
        protected ActionParameters ActionParameters;
        
        /// <summary>
        /// 遊戲行為數值已經設定
        /// </summary>
        protected bool HasSetValue = false;
        /// <summary>
        /// 行為目標對象
        /// </summary>
        protected CharacterBase Target;
        /// <summary>
        /// 行為的發起者
        /// </summary>
        protected CharacterBase Self;
        
        /// <summary>
        /// 基礎數值
        /// </summary>
        protected int BaseValue;
        /// <summary>
        /// 加成數值
        /// </summary>
        protected float MultiplierValue;
        /// <summary>
        /// 加成數量
        /// </summary>
        protected float MultiplierAmount;

        /// <summary>
        /// 加乘後的數值
        /// </summary>
        protected int AdditionValue
        {
            get { return Mathf.RoundToInt(BaseValue + MultiplierAmount * MultiplierValue); }
        }

        /// <summary>
        /// 行為所需時間
        /// </summary>
        public float Duration = 0;
        /// <summary>
        /// 能力類型
        /// </summary>
        protected PowerType PowerType;
        /// <summary>
        /// 根據答對數，產生不同行動
        /// </summary>
        protected AnswerOutcomeType AnswerOutcomeType;

        /// <summary>
        /// 特效出現位置
        /// </summary>
        protected Transform FXTransform;
        /// <summary>
        /// 特效類型
        /// </summary>
        public FxType FxType;
        /// <summary>
        /// 音效類型
        /// </summary>
        public AudioActionType AudioActionType;


        private DamageInfo _damageInfo;

        /// <summary>
        /// 傷害類型
        /// </summary>
        protected DamageInfo DamageInfo;
        
        /// <summary>
        /// 起始的卡組
        /// </summary>
        public PileType SourcePile;
        /// <summary>
        /// 目標的卡組
        /// </summary>
        public PileType TargetPile;
        /// <summary>
        /// 目標卡牌
        /// </summary>
        public CardData TargetCardData;

        /// <summary>
        /// 觸發的行動
        /// </summary>
        public List<ActionData> TriggerActionList;



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


        /// <summary>
        /// 依據參數設定行為數值
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void SetValue(ActionParameters parameters)
        {
            ActionParameters = parameters;
            ActionData data = parameters.ActionData;
            BaseValue = data.ActionValue;
            MultiplierValue = data.AdditionValue;
            MultiplierAmount = 0;
            PowerType = data.PowerType; 
            Duration = data.ActionDelay;
            AnswerOutcomeType = data.AnswerOutcomeType;
            
            SourcePile = data.SourcePile;
            TargetPile = data.TargetPile;
            TargetCardData = data.TargetCardData;

            Self = parameters.Self;
            Target = parameters.Target;

            DamageInfo = new DamageInfo(parameters);

            TriggerActionList = data.TriggerActionList;
            
            HasSetValue = true;
        }

        public virtual void SetValue(DamageInfo info)
        {
            DamageInfo = info;
            BaseValue = DamageInfo.BaseValue;
            MultiplierValue = DamageInfo.MultiplierValue;
            MultiplierAmount = DamageInfo.MultiplierAmount;
            
            Target = info.Target;

            HasSetValue = true;
        }

        public virtual void SetValue(ApplyPowerParameters parameters)
        {
            BaseValue = parameters.Value;
            Target = parameters.Target;
            PowerType = parameters.PowerType;
            
            HasSetValue = true;
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public abstract void DoAction();
        
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
        protected void PlayFx()
        {
            // TODO 設定 FX 預設產生處
            if (Target != null)
            {
                FxManager.PlayFx(Target.transform, FxType);
            }
                
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        protected void PlayAudio()
        {
            AudioManager.PlayOneShot(AudioActionType);
        }
        

        /// <summary>
        /// 確認遊戲行為數值是否已經設定
        /// </summary>
        protected void CheckHasSetValue()
        {
            if (!HasSetValue)
            {
                Debug.LogError($"{this.GetType()} hasn't set value.\n Please Call SetValue");
            }
        }

        /// <summary>
        /// 行為目標是為否空
        /// </summary>
        /// <returns></returns>
        protected bool IsTargetNull()
        {
            if (!Target)
            {
                Debug.Log($"{GetType()} 找不到Target");
            }

            return !Target;
        }

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
            DamageInfo.BaseValue = BaseValue;
            DamageInfo.MultiplierValue = MultiplierValue;
            DamageInfo.MultiplierAmount = MultiplierAmount;
            return DamageInfo;
        }
        
        
        public override string ToString()
        {
            return $"{GetType().Name} \n {nameof(Target)}: {Target}, {nameof(Self)}: {Self}, {nameof(AdditionValue)}: {AdditionValue}";
        }
    }
}