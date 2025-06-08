using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Display;
using Combat;
using Effect.Parameters;
using Feedback;
using GameListener;
using Log;
using Managers;
using Power;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UI;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// 角色
    /// 文件：https://hackmd.io/@Cobra3279/HJK2qpy9h/%2FNLPqD8z3QaO6heSAZ-rBXA
    /// </summary>
    public abstract class CharacterBase : SerializedMonoBehaviour
    {
        /// <summary>
        /// 角色類型
        /// </summary>
        [SerializeField] private CharacterType characterType;
        [Required]
        [SerializeField] private Transform textSpawnRoot;
        [Required]
        [SerializeField] protected CharacterCanvas _characterCanvas;

        public CharacterCanvas CharacterCanvas => _characterCanvas;
        
        #region Cache

        /// <summary>
        /// 角色數值
        /// </summary>
        [SerializeField]
        protected CharacterStats CharacterStats; 
        /// <summary>
        /// 文字特效生成處
        /// </summary>
        public Transform TextSpawnRoot => textSpawnRoot;
        protected GameManager GameManager => GameManager.Instance;


        #endregion
        
        
        #region 事件

        /// <summary>
        /// 事件：玩家死亡時觸發
        /// </summary>
        public Action<DamageInfo> OnDeath;

        /// <summary>
        /// 事件：當生命值改變時觸發
        /// </summary>
        public Action<int, int> OnHealthChanged;
        
        /// <summary>
        /// 被攻擊時。
        /// </summary>
        public Action<DamageInfo> OnAttacked;
        
        /// <summary>
        ///  事件: 當獲得能力時觸發
        /// </summary>
        public Action<PowerName, int> OnPowerApplied;
        
        /// <summary>
        ///  事件: 當能力數值改變時觸發
        /// </summary>
        public Action<PowerName, int> OnPowerChanged;
        
        /// <summary>
        /// 事件: 當能力數值增加時觸發
        /// </summary>
        public Action<PowerName, int> OnPowerIncreased;
        
        /// <summary>
        ///  事件: 當清除能力時觸發
        /// </summary>
        public Action<PowerName> OnPowerCleared;
        
        
        protected virtual void SubscribeEvent()
        {
            
            CombatManager.OnTurnStart += CharacterStats.HandleAllPowerOnTurnStart;
            CombatManager.OnTurnEnd += CharacterStats.HandleAllPowerOnTurnEnd;
            
            OnDeath += OnDeathAction;
        }

        protected virtual void UnsubscribeEvent()
        {
            CombatManager.OnTurnStart -= CharacterStats.HandleAllPowerOnTurnStart;
            CombatManager.OnTurnEnd -= CharacterStats.HandleAllPowerOnTurnEnd;
 
            OnDeath -= OnDeathAction;
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvent();
        }

        

        #endregion


        #region 特效

        [SerializeField] protected  IFeedback defaultAttackFeedback;
        [SerializeField] protected IFeedback beAttackFeedback;
        [SerializeField] protected PowerFeedback gainPowerFeedbackPrefab;
        [SerializeField] protected Transform powerFeedbackSpawn;
        [SerializeField] protected IFeedback onDeadFeedback;

        [ReadOnly]
        [SerializeField] private  Dictionary<string, IFeedback> feedbackDict = new Dictionary<string, IFeedback>();
        [SerializeField] protected List<IFeedback> Feedbacks;

        [Required]
        [SerializeField] private BlockFeedback blockFeedback;

        protected void SetUpFeedbackDict()
        {
            feedbackDict = new Dictionary<string, IFeedback>();
            foreach (var feedBack in Feedbacks)
            {
                feedbackDict.Add(feedBack.name, feedBack);
            }
        }


        public void PlayFeedback(string key)
        {
            if (feedbackDict.ContainsKey(key))
            {
                feedbackDict[key].Play();
            }
            else
            {
                Debug.LogError($"Character {name} does contain feedback {key}");
            }
        }
        
        public void PlayDefaultAttackFeedback()
        {
            defaultAttackFeedback?.Play();
        }

        public List<string> GetCustomFeedbackList()
        {
            var keys = new List<string>();
  
            foreach (var feedback in Feedbacks)
            {
                keys.Add(feedback.name);
            }

            return keys;
        }

        #endregion
        
        
        #region 傷害與血量
        /// <summary>
        /// 角色被攻擊時
        /// </summary>
        /// <param name="damageInfo"></param>
        public virtual void BeAttacked(DamageInfo damageInfo)
        {
            // 目標已死亡，停止攻擊
            if(CharacterStats.IsDeath) {return;}
            
            CharacterStats.BeAttacked(damageInfo);
            beAttackFeedback?.Play();
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"受到攻擊: {name}", 
                $"傷害資訊: {damageInfo}\n" +
                $"剩餘血量: {CharacterStats.CurrentHealth}");
            
            // 執行 GameEventListener(遊戲事件監聽器)，包含角色持有的能力、遺物
            var listeners = GetEventListeners();
            foreach (var listener in listeners)
            {
                listener.OnBeAttacked(damageInfo);
            }
        }

        /// <summary>
        /// 角色攻擊執行行為時
        /// </summary>
        /// <param name="damageInfo"></param>
        public void InvokeOnAttack(DamageInfo damageInfo, List<CharacterBase> targets)
        {
            // 執行 GameEventListener(遊戲事件監聽器)，包含角色持有的能力、遺物
            var listeners = GetEventListeners();
            foreach (var listener in listeners)
            {
                listener.OnAttack(damageInfo, targets);
            }
        }

        public void Heal(int value)
        {
            // 如果目標已死亡，停止行動
            if(CharacterStats.IsDeath) {return;}
            
            CharacterStats.Heal(value);
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"回血: {name}", 
                $"剩餘血量: {CharacterStats.CurrentHealth}");
        }

        /// <summary>
        /// 直接設定死亡
        /// </summary>
        public void SetDeath()
        {
            CharacterStats.SetDeath();
        }
        
        /// <summary>
        /// 角色死亡時執行
        /// </summary>
        /// <param name="damageInfo"></param>
        protected virtual void OnDeathAction(DamageInfo damageInfo)
        {
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"死亡 {name}", 
                $"傷害資訊: {damageInfo}");

            // 執行 GameEventListener(遊戲事件監聽器)，包含角色持有的能力、遺物
            var listeners = GetEventListeners();
            foreach (var listener in listeners)
            {
                listener.OnDead(damageInfo);
            }
            
            onDeadFeedback?.Play();
            UnsubscribeEvent();
        }


        public int GetMaxHealth()
        {
            return CharacterStats.MaxHealth;
        }

        public int GetHealth()
        {
            return CharacterStats.CurrentHealth;
        }

        /// <summary>
        /// 取得 GameEventListener(遊戲事件監聽器)
        /// 包含角色持有的能力、遺物
        /// </summary>
        /// <returns></returns>
        public List<GameEventListener> GetEventListeners()
        {
            var listener = new List<GameEventListener>();
            listener.AddRange(GetPowerDict().Values);
            listener.AddRange(ListenerGetter.GetRelicEventsListeners());
            
            return listener;
        }

        #endregion
        
        
        #region 能力 Power

        /// <summary>
        /// 賦予能力
        /// </summary>
        /// <param name="targetPower"></param>
        /// <param name="value"></param>
        public void ApplyPower(PowerName targetPower, int value, EffectSource effectSource)
        {
            if (CharacterStats.IsDeath) return;
            
            var (success, isNew) = CharacterStats.ApplyPower(targetPower, value);
            if (!success) return;

            // 記錄日誌
            EventLogger.Instance.LogEvent(
                LogEventType.Combat,
                $"赋予能力: {targetPower} + {value} 给 {name}",
                $"{effectSource}"
            );
            
            PlayPowerFeedback(targetPower, value, isNew);
        }

        /// <summary>
        /// 播放特效的反饋動畫
        /// </summary>
        /// <param name="power"></param>
        /// <param name="value"></param>
        /// <param name="isNew"></param>
        private void PlayPowerFeedback(PowerName power, int value, bool isNew)
        {
            if (power == PowerName.Block)
            {
                // 播放格檔的動畫
                int currentBlockValue = GetPowerValue(PowerName.Block);
                blockFeedback.PlayBlockEffect(currentBlockValue, isNew, value < 0);
            }
            else
            {
                // 播放一般能力的動畫
                var powerFeedback = Instantiate(gainPowerFeedbackPrefab, powerFeedbackSpawn);
                powerFeedback.Play(power, value > 0);
            }
        }
        
        /// <summary>
        /// 將能力 x 倍數
        /// </summary>
        public void MultiplyPower(PowerName targetPower,int value)
        {
            // 如果目標已死亡，停止行動
            if(CharacterStats.IsDeath) {return;}
            
            CharacterStats.MultiplyPower(targetPower, value);
            gainPowerFeedbackPrefab.Play(targetPower, true);
        }

        /// <summary>
        /// 清除能力
        /// </summary>
        /// <param name="targetPower"></param>
        public void ClearPower(PowerName targetPower, EffectSource effectSource)
        {
            // 如果目標已死亡，停止行動
            if(CharacterStats.IsDeath) {return;}
            
            CharacterStats.ClearPower(targetPower);
            
            // 记录日志
            EventLogger.Instance.LogEvent(LogEventType.Combat, 
                $"清除能力: {targetPower} 在 {name}",
                $"{effectSource}");

            // 播放反馈效果
            if (targetPower == PowerName.Block)
            {
                blockFeedback.PlayRemoveBlock();
            }
            else
            {
                var powerFeedback = Instantiate(gainPowerFeedbackPrefab, powerFeedbackSpawn);
                powerFeedback.Play(targetPower, false);
            }
        }

        /// <summary>
        /// 清除所有能力
        /// </summary>
        public void ClearAllPower()
        {
            CharacterStats.ClearAllPower();
        }
        
        /// <summary>
        /// 是否持有能力
        /// </summary>
        /// <param name="targetPower"></param>
        /// <returns></returns>
        public bool HasPower(PowerName targetPower)
        {
            return CharacterStats.PowerDict.ContainsKey(targetPower);
        }

        /// <summary>
        /// 取得能力的數值
        /// </summary>
        /// <param name="targetPower"></param>
        /// <returns></returns>
        public int GetPowerValue(PowerName targetPower)
        {
            if (CharacterStats.PowerDict.TryGetValue(targetPower, out var value))
            {
                return value.Amount;
            }

            return 0;
        }
        
        public Dictionary<PowerName, PowerBase> GetPowerDict()
        {
            if (CharacterStats == null) return new Dictionary<PowerName, PowerBase>();
            
            return CharacterStats.PowerDict;
        }

        public List<GameEventListener> GetPowerListeners()
        {
            var listeners = new List<GameEventListener>();
            listeners.AddRange(CharacterStats.PowerDict.Values);

            return listeners;
        }
        
        #endregion

        #region 工具

        public CharacterStats GetCharacterStats()
        {
            return CharacterStats;
        }

        
        /// <summary>
        /// 檢查此玩家是否為指定的類別(Enemy, Ally)
        /// </summary>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public bool IsCharacterType(CharacterType checkType)
        {
            return characterType == checkType;
        }

        #endregion

        public override string ToString()
        {
            return $"{nameof(characterType)}: {characterType}\n{nameof(CharacterStats)}: {CharacterStats}";
        }
    }
}