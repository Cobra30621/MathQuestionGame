using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Power;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Characters
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
        
        public Action OnTriggerAction;
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
        private CharacterBase owner;
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public bool IsStunned { get;  set; }
        public bool IsDeath { get; private set; }
       
        public Action OnDeath;
        public Action<int, int> OnHealthChanged;
        private readonly Action<PowerType,int> OnStatusChanged;
        private readonly Action<PowerType, int> OnStatusApplied;
        private readonly Action<PowerType> OnStatusCleared;
        public Action OnHealAction;
        public Action OnTakeDamageAction;
        public Action OnShieldGained;
        
        public readonly Dictionary<PowerType, StatusStats> StatusDict = new Dictionary<PowerType, StatusStats>();
        public Dictionary<PowerType, PowerBase> PowerDict = new Dictionary<PowerType, PowerBase>();

        #region Setup
        public CharacterStats(int maxHealth, CharacterCanvas characterCanvas, CharacterBase characterBase)
        {
            owner = characterBase;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            SetAllStatus();
            
            OnHealthChanged += characterCanvas.UpdateHealthInfo;
            OnStatusChanged += characterCanvas.UpdateStatusText;
            OnStatusApplied += characterCanvas.ApplyStatus;
            OnStatusCleared += characterCanvas.ClearStatus;
        }

        private void SetAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(PowerType)).Length; i++)
                StatusDict.Add((PowerType) i, new StatusStats((PowerType) i, 0));

            StatusDict[PowerType.Poison].DecreaseOverTurn = true;
            StatusDict[PowerType.Poison].OnTriggerAction += DamagePoison;

            StatusDict[PowerType.Block].ClearAtNextTurn = true;

            StatusDict[PowerType.Strength].CanNegativeStack = true;
            StatusDict[PowerType.Dexterity].CanNegativeStack = true;
            
            StatusDict[PowerType.Stun].DecreaseOverTurn = true;
            StatusDict[PowerType.Stun].OnTriggerAction += CheckStunStatus;
            
            StatusDict[PowerType.Vulnerable].DecreaseOverTurn = true;
            StatusDict[PowerType.Weak].DecreaseOverTurn = true;
            
        }
        #endregion
        
        #region Public Methods
        public void ApplyStatus(PowerType targetPower,int value)
        {
            if (PowerDict.ContainsKey(targetPower))
            {
                PowerDict[targetPower].StackPower(value);
                OnStatusChanged?.Invoke(targetPower, PowerDict[targetPower].Value);
            }
            else
            {
                PowerBase powerBase = PowerFactory.GetPower(targetPower);
                powerBase.Owner = owner;
                powerBase.StackPower(value);
                
                
                PowerDict.Add(targetPower, powerBase);
                OnStatusApplied?.Invoke(targetPower, PowerDict[targetPower].Value);
            }
        }

        public void TriggerAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(PowerType)).Length; i++)
                TriggerStatus((PowerType) i);
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
                if (StatusDict[PowerType.Block].IsActive)
                {
                    ApplyStatus(PowerType.Block,-value);

                    remainingDamage = 0;
                    if (StatusDict[PowerType.Block].StatusValue <= 0)
                    {
                        remainingDamage = StatusDict[PowerType.Block].StatusValue * -1;
                        ClearStatus(PowerType.Block);
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
            foreach (var status in StatusDict)
                ClearStatus(status.Key);
        }
           
        public void ClearStatus(PowerType targetPower)
        {
            StatusDict[targetPower].IsActive = false;
            StatusDict[targetPower].StatusValue = 0;
            OnStatusCleared?.Invoke(targetPower);
        }

        #endregion

        #region Private Methods
        private void TriggerStatus(PowerType targetPower)
        {
            StatusDict[targetPower].OnTriggerAction?.Invoke();
            
            //One turn only statuses
            if (StatusDict[targetPower].ClearAtNextTurn)
            {
                ClearStatus(targetPower);
                OnStatusChanged?.Invoke(targetPower, StatusDict[targetPower].StatusValue);
                return;
            }
            
            //Check status
            if (StatusDict[targetPower].StatusValue <= 0)
            {
                if (StatusDict[targetPower].CanNegativeStack)
                {
                    if (StatusDict[targetPower].StatusValue == 0 && !StatusDict[targetPower].IsPermanent)
                        ClearStatus(targetPower);
                }
                else
                {
                    if (!StatusDict[targetPower].IsPermanent)
                        ClearStatus(targetPower);
                }
            }
            
            if (StatusDict[targetPower].DecreaseOverTurn) 
                StatusDict[targetPower].StatusValue--;
            
            if (StatusDict[targetPower].StatusValue == 0)
                if (!StatusDict[targetPower].IsPermanent)
                    ClearStatus(targetPower);
            
            OnStatusChanged?.Invoke(targetPower, StatusDict[targetPower].StatusValue);
        }
        
     
        private void DamagePoison()
        {
            if (StatusDict[PowerType.Poison].StatusValue<=0) return;
            Damage(StatusDict[PowerType.Poison].StatusValue,true);
        }
        
        public void CheckStunStatus()
        {
            // if (PowerDict[PowerType.Stun].Value <= 0)
            // {
            //     IsStunned = false;
            //     return;
            // }
            //
            // IsStunned = true;
        }
        
        #endregion
    }
}