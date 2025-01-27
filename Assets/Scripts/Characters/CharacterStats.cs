using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Display;
using Combat;
using Effect.Parameters;
using Managers;
using Power;
using UnityEngine;

namespace Characters
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
        
        public CharacterStats(int currentHealth, int maxHealth, CharacterBase characterBase,
            CharacterCanvas characterCanvas)
        {
            owner = characterBase;
            MaxHealth = maxHealth;
            
            SetCharacterCanvasEvent(characterCanvas);
            SetCurrentHealth(currentHealth);
        }

        public void SetCharacterCanvasEvent(CharacterCanvas characterCanvas)
        {
            owner.OnPowerApplied += characterCanvas.ApplyStatus;
            owner.OnPowerChanged += characterCanvas.UpdateStatusText;
            owner.OnPowerCleared += characterCanvas.ClearStatus;
            owner.OnHealthChanged += characterCanvas.UpdateHealthInfo;
        }

        
        #endregion
        
        #region Power Management
        /// <summary>
        /// 賦予角色能力或疊加現有能力
        /// </summary>
        /// <param name="targetPower">目標能力名稱</param>
        /// <param name="value">能力值或疊加值</param>
        /// <returns>
        /// Item1 (success): 是否成功
        /// Item2 (isNew): 是否為新增的能力
        /// </returns>
        public (bool success, bool isNew) ApplyPower(PowerName targetPower, int value)
        {
            if (value == 0) return (true, false);
            
            if (PowerDict.TryGetValue(targetPower, out var existingPower))
            {
                existingPower.StackPower(value);
                return (true, false);
            }

            var newPower = PowerGenerator.GetPower(targetPower);
            if (newPower == null) return (false, false);

            InitializeNewPower(newPower, value);
            return (true, true);
        }

        private void InitializeNewPower(PowerBase power, int value)
        {
            PowerDict.Add(power.PowerName, power);
            power.SetOwner(owner);
            power.StackPower(value);
            power.Init();
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

            GameManager.Instance.StartCoroutine(UpdatePowerStatusCoroutine());
        }

        /// <summary>
        /// 更新能力的流程
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdatePowerStatusCoroutine()
        {
            var copyPowerDict = new Dictionary<PowerName, PowerBase> (PowerDict);
            foreach (PowerBase power in copyPowerDict.Values)
            {
                power.UpdateStatusOnTurnStart();
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        
        #endregion

        #region Health and Damage
        
        /// <summary>
        /// 設置生命值
        /// </summary>
        /// <param name="targetCurrentHealth"></param>
        private void SetCurrentHealth(int targetCurrentHealth)
        {
            int oldHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(targetCurrentHealth, 0, MaxHealth);
            
            if (oldHealth != CurrentHealth)
            {
                owner.OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
                NotifyHealthChangeToListeners();
            }
        }

        private void NotifyHealthChangeToListeners()
        {
            var gameEventListeners = owner.GetEventListeners();
            foreach (var eventListener in gameEventListeners)
            {
                eventListener.OnHealthChanged(CurrentHealth, MaxHealth);
            }
        }

        /// <summary>
        /// 治療
        /// </summary>
        /// <param name="value"></param>
        public void Heal(int value)
        {
            SetCurrentHealth(CurrentHealth + value);
        }

        /// <summary>
        /// 受到傷害
        /// </summary>
        /// <param name="damageInfo"></param>
        public void BeAttacked(DamageInfo damageInfo)
        {
            if (IsDeath) return;

            int damageValue = damageInfo.GetDamageValue();
            int afterBlockDamage = damageInfo.GetAfterBlockDamage();
            
            HandleBlockDamage(damageValue, afterBlockDamage, damageInfo.EffectSource);
            HandleHealthDamage(afterBlockDamage);
            
            owner.OnAttacked?.Invoke(damageInfo);
            CheckIsDeath(damageInfo);
        }
        
        private void HandleBlockDamage(int totalDamage, int afterBlockDamage, EffectSource source)
        {
            int blockDamage = totalDamage - afterBlockDamage;
            if (blockDamage > 0)
            {
                owner.ApplyPower(PowerName.Block, -blockDamage, source);
            }
        }

        private void HandleHealthDamage(int damage)
        {
            if (damage > 0)
            {
                SetCurrentHealth(CurrentHealth - damage);
            }
        }
        
        /// <summary>
        /// 直接設定怪物死亡
        /// </summary>
        public void SetDeath()
        {
            CurrentHealth = 0;
            CheckIsDeath(new DamageInfo(0, new EffectSource()));
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