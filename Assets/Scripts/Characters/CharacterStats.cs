using System;
using System.Collections.Generic;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Characters
{
    public class StatusStats
    { 
        public PowerType PowerType { get; set; }
        public int StatusValue { get; set; }
        public bool DecreaseOverTurn { get; set; } // If true, decrease on turn end
        public bool IsPermanent { get; set; } // If true, status can not be cleared during combat
        public bool IsActive { get; set; }
        public bool CanNegativeStack { get; set; }
        public bool ClearAtNextTurn { get; set; }
        
        public StatusStats(PowerType powerType,int statusValue,bool decreaseOverTurn = false, bool isPermanent = false,bool isActive = false,bool canNegativeStack = false,bool clearAtNextTurn = false)
        {
            PowerType = powerType;
            StatusValue = statusValue;
            DecreaseOverTurn = decreaseOverTurn;
            IsPermanent = isPermanent;
            IsActive = isActive;
            CanNegativeStack = canNegativeStack;
            ClearAtNextTurn = clearAtNextTurn;
        }
    }
    public class CharacterStats
    {
        private readonly CharacterBase owner;
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public bool IsStunned { get;  set; }
        public bool IsDeath { get; private set; }
       
        public System.Action OnDeath;
        public Action<int, int> OnHealthChanged;
        public Action<PowerType, int> OnPowerApplied;
        public Action<PowerType, int> OnPowerChanged;
        public Action<PowerType> OnPowerCleared;
        
        public System.Action OnHealAction;
        public System.Action OnTakeDamageAction;
        public System.Action OnShieldGained;
        
        public EventManager EventManager => EventManager.Instance;
        
        public readonly Dictionary<PowerType, PowerBase> PowerDict = new Dictionary<PowerType, PowerBase>();

        #region Setup
        public CharacterStats(int maxHealth, CharacterCanvas characterCanvas, CharacterBase characterBase)
        {
            owner = characterBase;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            
            OnPowerApplied += characterCanvas.ApplyStatus;
            OnPowerChanged += characterCanvas.UpdateStatusText;
            OnPowerCleared += characterCanvas.ClearStatus;
            
            OnHealthChanged += characterCanvas.UpdateHealthInfo;
        }

        
        #endregion
        
        #region Public Methods
        public void ApplyPower(PowerType targetPower,int value)
        {
            // Debug.Log($"{owner.name} apply {targetPower} {value}");
            if (PowerDict.ContainsKey(targetPower))
            {
                PowerDict[targetPower].StackPower(value);
            }
            else
            {
                PowerBase powerBase = PowerFactory.GetPower(targetPower);
                powerBase.Owner = owner;
                powerBase.StackPower(value);
                
                PowerDict.Add(targetPower, powerBase);
            }
        }

        public void ReducePower(PowerType targetPower,int value)
        {
            PowerDict[targetPower].ReducePower(value);
        }

        public void HandleAllPowerOnTurnStart()
        {
            foreach (PowerBase power in PowerDict.Values)
            {
                power.OnTurnStarted();
            }
        }
        
        public void SetCurrentHealth(int targetCurrentHealth)
        {
            CurrentHealth = targetCurrentHealth <=0 ? 1 : targetCurrentHealth;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        } 
        
        public void Heal(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth>MaxHealth)  CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }
        
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
        
        public void IncreaseMaxHealth(int value)
        {
            MaxHealth += value;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }

        public void ClearAllStatus()
        {
            foreach (var power in PowerDict)
                ClearPower(power.Key);
        }
           
        public void ClearPower(PowerType targetPower)
        {
            PowerDict[targetPower].ClearPower();
        }

        #endregion

    }
}