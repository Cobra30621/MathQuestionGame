﻿using System;
using System.Collections.Generic;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Characters
{
    /// <summary>
    /// 角色數值，並提供修改數值方式(受到傷害、給予傷害、回血等等)
    /// </summary>
    public class CharacterStats
    {
        /// <summary>
        /// 持有數值的角色
        /// </summary>
        private readonly CharacterBase owner;
        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxHealth { get; set; }
        /// <summary>
        /// 現在生命值
        /// </summary>
        public int CurrentHealth { get; set; }
        /// <summary>
        /// 是否暈眩
        /// </summary>
        public bool IsStunned { get;  set; }
        /// <summary>
        /// 是否死亡
        /// </summary>
        public bool IsDeath { get; private set; }
       
        /// <summary>
        /// 事件：玩家死亡時觸發
        /// </summary>
        public System.Action OnDeath;
        /// <summary>
        /// 事件：當生命值改變時觸發
        /// </summary>
        public Action<int, int> OnHealthChanged;
        /// <summary>
        ///  事件: 當獲得能力時觸發
        /// </summary>
        public Action<PowerType, int> OnPowerApplied;
        /// <summary>
        ///  事件: 當能力數值改變時觸發
        /// </summary>
        public Action<PowerType, int> OnPowerChanged;
        /// <summary>
        ///  事件: 當清除能力時觸發
        /// </summary>
        public Action<PowerType> OnPowerCleared;
        
        public System.Action OnTakeDamageAction;
        public System.Action OnShieldGained;
        
        public EventManager EventManager => EventManager.Instance;
        
        /// <summary>
        /// 持有的能力清單
        /// </summary>
        public readonly Dictionary<PowerType, PowerBase> PowerDict = new Dictionary<PowerType, PowerBase>();

        #region Setup 初始設定
        public CharacterStats(int maxHealth, CharacterBase characterBase)
        {
            owner = characterBase;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            
            
        }

        public void SetCharacterCanvasEvent(CharacterCanvas characterCanvas)
        {
            OnPowerApplied += characterCanvas.ApplyStatus;
            OnPowerChanged += characterCanvas.UpdateStatusText;
            OnPowerCleared += characterCanvas.ClearStatus;
            OnHealthChanged += characterCanvas.UpdateHealthInfo;
        }

        
        #endregion
        
        #region Public Methods
        /// <summary>
        /// 賦予能力
        /// </summary>
        /// <param name="targetPower"></param>
        /// <param name="value"></param>
        public void ApplyPower(PowerType targetPower,int value)
        {
            // Debug.Log($"{owner.name} apply {targetPower} {value}");
            if (PowerDict.ContainsKey(targetPower))
            {
                PowerDict[targetPower].StackPower(value);
            }
            else
            {
                PowerBase powerBase = PowerGenerator.GetPower(targetPower);
                powerBase.Owner = owner;
                powerBase.StackPower(value);
                
                PowerDict.Add(targetPower, powerBase);
            }
        }

        /// <summary>
        /// 降低能力
        /// </summary>
        /// <param name="targetPower"></param>
        /// <param name="value"></param>
        public void ReducePower(PowerType targetPower,int value)
        {
            PowerDict[targetPower].ReducePower(value);
        }

        /// <summary>
        /// 回合開始時，通知持有的能力
        /// </summary>
        public void HandleAllPowerOnTurnStart()
        {
            var copyPowerDict = new Dictionary<PowerType, PowerBase> (PowerDict);
            foreach (PowerBase power in copyPowerDict.Values)
            {
                power.OnTurnStarted();
            }
        }
        
        /// <summary>
        /// 設置生命值
        /// </summary>
        /// <param name="targetCurrentHealth"></param>
        public void SetCurrentHealth(int targetCurrentHealth)
        {
            CurrentHealth = targetCurrentHealth <=0 ? 1 : targetCurrentHealth;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        } 
        
        /// <summary>
        /// 治療
        /// </summary>
        /// <param name="value"></param>
        public void Heal(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth>MaxHealth)  CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }
        
        /// <summary>
        /// 受到傷害
        /// </summary>
        /// <param name="value"></param>
        /// <param name="canPierceArmor"></param>
        public void Damage(int value, bool canPierceArmor = false)
        {
            if (IsDeath) return;
            OnTakeDamageAction?.Invoke();
            var remainingDamage = value;
            
            if (!canPierceArmor)
            {
                if (PowerDict.ContainsKey(PowerType.Block))
                {
                    ApplyPower(PowerType.Block,-value);

                    remainingDamage = 0;
                    if (PowerDict[PowerType.Block].Value <= 0)
                    {
                        remainingDamage = PowerDict[PowerType.Block].Value * -1;
                        ClearPower(PowerType.Block);
                    }
                }
            }
            
            CurrentHealth -= remainingDamage;
            
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeath?.Invoke();
                IsDeath = true;
            }
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }
        
        /// <summary>
        /// 增加最大生命值
        /// </summary>
        /// <param name="value"></param>
        public void IncreaseMaxHealth(int value)
        {
            MaxHealth += value;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }

        /// <summary>
        /// 清除所有能力
        /// </summary>
        public void ClearAllPower()
        {
            foreach (var power in PowerDict)
                ClearPower(power.Key);
        }
        
        /// <summary>
        /// 清除能力
        /// </summary>
        /// <param name="targetPower"></param>
        public void ClearPower(PowerType targetPower)
        {
            PowerDict[targetPower].ClearPower();
        }

        #endregion

    }
}