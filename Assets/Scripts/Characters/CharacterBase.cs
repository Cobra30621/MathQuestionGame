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
        [Header("Base settings")]
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
        protected CombatManager CombatManager => CombatManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

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
            
            OnDeath += OnDeathAction;
        }

        protected virtual void UnsubscribeEvent()
        {
            CombatManager.OnTurnStart -= CharacterStats.HandleAllPowerOnTurnStart;
            ClearAllPower();

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
        
        
        
        
        
        #region Damage
        /// <summary>
        /// 角色被攻擊時
        /// </summary>
        /// <param name="damageInfo"></param>
        public virtual void BeAttacked(DamageInfo damageInfo)
        {
            CharacterStats.BeAttacked(damageInfo);
            beAttackFeedback?.Play();
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, $"收到攻擊: {name}", 
                $"傷害資訊: {damageInfo}\n" +
                $"剩餘血量: {CharacterStats.CurrentHealth}");
            
            // 執行 GameEventListener(遊戲事件監聽器)，包含角色持有的能力、遺物
            var listeners = GetEventListeners();
            foreach (var listener in listeners)
            {
                listener.OnAttacked(damageInfo);
            }
        }

        public void Heal(int value)
        {
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
        
        
        #region Power

        /// <summary>
        /// 賦予能力
        /// </summary>
        /// <param name="targetPower"></param>
        /// <param name="value"></param>
        public void ApplyPower(PowerName targetPower,int value, EffectSource effectSource)
        {
            var (haveFindPower, isNewPower) = CharacterStats.ApplyPower(targetPower, value);

            EventLogger.Instance.LogEvent(LogEventType.Combat, 
                $"賦予能力: {targetPower} + {value} 給 {name}",
                $"{effectSource}");
            
            // 沒找到能力，不播特效
            if (!haveFindPower)
            {
                return;
            }
            
            if (targetPower == PowerName.Block)
            {
                bool havePower = CharacterStats.PowerDict.TryGetValue(PowerName.Block, out PowerBase power);
                bool clearPower = !havePower;
                int blockValue =  clearPower ? 0 : power.Amount;                
                
                PlayBlockFeedback(isNewPower, clearPower, value < 0, blockValue);
            }
            else
            {
                var powerFeedback = Instantiate(gainPowerFeedbackPrefab, powerFeedbackSpawn);
                powerFeedback.Play(targetPower, value > 0);
            }
        }

        private void PlayBlockFeedback(bool isNewPower, bool isClearPower, bool isNegative, int amount)
        {
            if (isNewPower)
            {
                blockFeedback.PlayGainBlock(amount);
            }
            else
            {
                if (isClearPower)
                {
                    blockFeedback.PlayRemoveBlock();
                }
                else if (isNegative)
                {
                    blockFeedback.PlayReduceBlock(amount);
                }
                else
                {
                    blockFeedback.PlayBlockChange(amount);
                }
            }
        }
        
        /// <summary>
        /// 將能力 x 倍數
        /// </summary>
        public void MultiplyPower(PowerName targetPower,int value)
        {
            CharacterStats.MultiplyPower(targetPower, value);
            gainPowerFeedbackPrefab.Play(targetPower, true);
        }

        /// <summary>
        /// 清除能力
        /// </summary>
        /// <param name="targetPower"></param>
        public void ClearPower(PowerName targetPower, EffectSource effectSource)
        {
            CharacterStats.ClearPower(targetPower);
            
            EventLogger.Instance.LogEvent(LogEventType.Combat, 
                $"清除能力: {targetPower} 在 {name}",
                $"{effectSource}");

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
            return CharacterStats.PowerDict.Values
                .Select(power => power as GameEventListener).ToList();
        }
        
        #endregion

        
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

        public override string ToString()
        {
            return $"{nameof(characterType)}: {characterType}\n{nameof(CharacterStats)}: {CharacterStats}";
        }
    }
}