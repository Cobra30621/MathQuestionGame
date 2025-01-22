using Characters;
using Combat;
using Effect.Parameters;
using GameListener;
using UnityEngine;

namespace Power
{
    /// <summary>
    /// 能力（ex: 力量、易傷、中毒）的基底 class
    /// </summary>
    public abstract class PowerBase : GameEventListener
    {
        #region 能力可設定參數

        /// <summary>
        /// 能力類型
        /// </summary>
        public abstract PowerName PowerName { get; }
        
        /// <summary>
        /// 回合結束時數值 - 1
        /// </summary>
        public bool DecreaseOverTurn;
        
        /// <summary>
        /// 數值可以是負數
        /// </summary>
        public bool CanNegativeStack;

        
        #endregion

        #region 暫存參數
        /// <summary>
        /// 能力數值
        /// </summary>
        public int Amount;
        
        /// <summary>
        /// 能力持有者
        /// </summary>
        public CharacterBase Owner;
        
        #endregion
        
        
        #region SetUp

        public virtual void SetOwner(CharacterBase owner)
        {
            Owner = owner;
        }

        public virtual void Init()
        {
        }

        #endregion


        #region 能力數值控制
        
        /// <summary>
        /// 增加能力數值
        /// </summary>
        /// <param name="stackAmount"></param>
        public virtual void StackPower(int stackAmount)
        {
            // 已經有的能力
            if (Amount != 0)
            {
                SetPowerAmount(Amount + stackAmount);
            }
            // 新增的能力
            else
            { 
                
                Owner.OnPowerApplied?.Invoke(PowerName, Amount);
                SetPowerAmount(stackAmount);
            }

            // 能力數值提升
            if (stackAmount > 0)
            {
                Owner.OnPowerIncreased?.Invoke(PowerName, stackAmount);
            }

            CheckClearPower();
        }

        /// <summary>
        /// 設定能力數值
        /// </summary>
        /// <param name="amount"></param>
        public virtual void SetPowerAmount(int amount)
        {
            Amount = amount;
            DoOnPowerChanged(amount);
            Owner.OnPowerChanged?.Invoke(PowerName, Amount);
        }

        /// <summary>
        /// 將能力 x 倍數
        /// </summary>
        public virtual void MultiplyPower(int multiplyAmount)
        {
            int addAmount = Mathf.RoundToInt(Amount * (multiplyAmount - 1));
            StackPower(addAmount);
        }
        

        /// <summary>
        /// 檢查要不要清除能力
        /// </summary>
        private void CheckClearPower()
        {
            //Check status
            if (Amount <= 0)
            {
                if (CanNegativeStack)
                {
                    if (Amount == 0)
                        Owner.ClearPower(PowerName, GetEffectSource());
                }
                else
                {
                    Owner.ClearPower(PowerName, GetEffectSource());
                }
            }
        }

        /// <summary>
        /// 清除能力
        /// </summary>
        public void ClearPower()
        {
            Amount = 0;
            Owner.GetPowerDict().Remove(PowerName);
            Owner.OnPowerCleared.Invoke(PowerName);
            DoOnPowerClear();
        }

        #endregion


        #region 事件觸發
        
        /// <summary>
        /// 角色回合開始時，更新能力
        /// </summary>
        public virtual void UpdateStatusOnTurnStart()
        {
            if (DecreaseOverTurn)
                StackPower(-1);
        }
        
        /// <summary>
        /// 能力層數改變時，要執行的方法
        /// </summary>
        /// <param name="amount"></param>
        public virtual void DoOnPowerChanged(int amount)
        {
        }

        /// <summary>
        /// 能力清除時，要執行的方法
        /// </summary>
        public virtual void DoOnPowerClear()
        {
        }

        
        #endregion


        #region 工具

        public bool IsCharacterTurn(TurnInfo info)
        {
            return Owner.IsCharacterType(info.CharacterType);
        }

        protected EffectSource GetEffectSource()
        {
            return new EffectSource()
            {
                SourceType = SourceType.Power,
                SourcePower = PowerName,
                SourceCharacter = Owner
            };
        }
        
        /// <summary>
        /// 更新敵人的意圖數值，主要用於會改變攻擊力的能力
        /// </summary>
        protected void UpdateEnemyIntentionDisplay()
        {
            // 如果是敵人獲得力量，更新意圖顯示
            if (Owner.IsCharacterType(CharacterType.Enemy))
            {
                var enemy = (Characters.Enemy.Enemy) Owner;
                enemy.UpdateIntentionDisplay();
            }
        }


        #endregion


        public override string ToString()
        {
            return $"{nameof(PowerName)}: {PowerName}, {nameof(Amount)}: {Amount}";
        }
    }
}