using System;
using System.Collections.Generic;
using System.Linq;
using Action.Parameters;
using Combat;
using Newtonsoft.Json;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
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
        private CharacterBase owner;

        #region 參數

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxHealth;
        /// <summary>
        /// 現在生命值
        /// </summary>
        public int CurrentHealth ;
        /// <summary>
        /// 是否暈眩
        /// </summary>
        public bool IsStunned ;
        /// <summary>
        /// 是否死亡
        /// </summary>
        public bool IsDeath ;
        
        /// <summary>
        /// 持有的能力清單
        /// </summary>
        public readonly Dictionary<PowerName, PowerBase> PowerDict = new Dictionary<PowerName, PowerBase>();

        #endregion

        
        #region Setup 初始設定
        public CharacterStats(int maxHealth, CharacterBase characterBase, CharacterCanvas characterCanvas)
        {
            owner = characterBase;
            MaxHealth = maxHealth;
            
            SetCharacterCanvasEvent(characterCanvas);
            SetCurrentHealth(maxHealth);
        }

        public void SetCharacterCanvasEvent(CharacterCanvas characterCanvas)
        {
            owner.OnPowerApplied += characterCanvas.ApplyStatus;
            owner.OnPowerChanged += characterCanvas.UpdateStatusText;
            owner.OnPowerCleared += characterCanvas.ClearStatus;
            owner.OnHealthChanged += characterCanvas.UpdateHealthInfo;
        }

        
        #endregion
        
        #region Power 能力
        /// <summary>
        /// 賦予能力
        /// </summary>
        /// <param name="targetPower"></param>
        /// <param name="value"></param>
        public bool ApplyPower(PowerName targetPower,int value)
        {
            bool isNewPower = false;
            if (PowerDict.TryGetValue(targetPower, out var power))
            {
                power.StackPower(value);
            }
            else
            {
                PowerBase powerBase = PowerGenerator.GetPower(targetPower);
                PowerDict.Add(targetPower, powerBase);
                powerBase.SetOwner(owner);
                powerBase.SubscribeAllEvent();
                powerBase.StackPower(value);
                powerBase.Init();
                
                isNewPower = true;
            }
            
            return isNewPower;
        }

        
        /// <summary>
        /// 將能力 x 倍數
        /// </summary>
        public void MultiplyPower(PowerName targetPower,int value)
        {
            if(PowerDict.TryGetValue(targetPower, out var power))
                power.MultiplyPower(value);
        }


        /// <summary>
        /// 清除所有能力
        /// </summary>
        public void ClearAllPower()
        {
            Dictionary<PowerName, PowerBase> copyPowerDict = new Dictionary<PowerName, PowerBase>(PowerDict);
            foreach (var power in copyPowerDict)
                ClearPower(power.Key);
        }
        
        /// <summary>
        /// 清除能力
        /// </summary>
        /// <param name="targetPower"></param>
        public void ClearPower(PowerName targetPower)
        {
            if (PowerDict.TryGetValue(targetPower, out var value))
            {
                value.ClearPower();
            }
        }
        
        /// <summary>
        /// 角色回合開始時，通知持有的能力更新狀態
        /// </summary>
        public void HandleAllPowerOnTurnStart(TurnInfo info)
        {
            if (!owner.IsCharacterType(info.CharacterType))
            {
                return;
            }
            
            var copyPowerDict = new Dictionary<PowerName, PowerBase> (PowerDict);
            foreach (PowerBase power in copyPowerDict.Values)
            {
                power.UpdatePowerStatus();
            }
        }
        
        #endregion

        #region Health and Damage
        
        /// <summary>
        /// 設置生命值
        /// </summary>
        /// <param name="targetCurrentHealth"></param>
        public void SetCurrentHealth(int targetCurrentHealth)
        {
            CurrentHealth = targetCurrentHealth <=0 ? 1 : targetCurrentHealth;
            owner.OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        } 
        
        /// <summary>
        /// 治療
        /// </summary>
        /// <param name="value"></param>
        public void Heal(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth>MaxHealth)  CurrentHealth = MaxHealth;
            owner.OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }

        /// <summary>
        /// 受到傷害
        /// </summary>
        /// <param name="damageInfo"></param>
        public void BeAttacked(DamageInfo damageInfo)
        {
            if (IsDeath) return;
           

            var damageValue = damageInfo.GetDamageValue();
            var afterBlockDamage = damageInfo.GetAfterBlockDamage();
            
            CurrentHealth -= afterBlockDamage;
            if (afterBlockDamage > 0)
            {
                owner.OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
            }

            int reduceBlockValue =  damageValue - afterBlockDamage;
            if (reduceBlockValue > 0)
            {
                owner.ApplyPower(PowerName.Block, -reduceBlockValue);
            }
            owner.OnAttacked?.Invoke(damageInfo);
            
            CheckIsDeath(damageInfo);
            
        }
        
        /// <summary>
        /// 直接設定怪物死亡
        /// </summary>
        public void SetDeath()
        {
            CurrentHealth = 0;
            CheckIsDeath(new DamageInfo(0, new ActionSource()));
        }

        /// <summary>
        /// 判斷是否死亡
        /// </summary>
        /// <param name="damageInfo"></param>
        private void CheckIsDeath(DamageInfo damageInfo)
        {
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                owner.OnDeath?.Invoke(damageInfo);
                IsDeath = true;
            }
        }
      
        
        /// <summary>
        /// 增加最大生命值
        /// </summary>
        /// <param name="value"></param>
        public void IncreaseMaxHealth(int value)
        {
            MaxHealth += value;
            owner.OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }

        

        #endregion


        public override string ToString()
        {
            return $"{nameof(MaxHealth)}: {MaxHealth}, {nameof(CurrentHealth)}: {CurrentHealth}, {nameof(IsStunned)}: {IsStunned}, {nameof(IsDeath)}: {IsDeath}\n" +
                   $"{nameof(PowerDict)}: {PowerDict.Aggregate("", (current, pair) => current + $"{pair.Value}, ".ToString())}";
        }
    }
    
}