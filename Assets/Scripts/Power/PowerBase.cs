﻿using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Power
{
    /// <summary>
    /// 能力（ex: 力量、易傷、中毒）的基底 class
    /// </summary>
    public abstract class PowerBase
    {
        /// <summary>
        /// 能力類型
        /// </summary>
        public abstract PowerType PowerType  { get;}
        /// <summary>
        /// 能力數值
        /// </summary>
        public int Value;
        /// <summary>
        /// 能力是否被觸發
        /// </summary>
        public bool IsActive;
        /// <summary>
        /// 回合結束時數值 - 1
        /// </summary>
        public bool DecreaseOverTurn;
        /// <summary>
        /// 永久能力(本場戰鬥)
        /// </summary>
        public bool IsPermanent; 
        /// <summary>
        /// 數值可以是負數
        /// </summary>
        public bool CanNegativeStack;
        /// <summary>
        /// 回合結束時清除能力
        /// </summary>
        public bool ClearAtNextTurn;
        /// <summary>
        /// 能力持有者
        /// </summary>
        public CharacterBase Owner;
        /// <summary>
        /// 計數器，用來計算如回合數、答對題數、使用卡片張數等等
        /// </summary>
        public int Counter;
        /// <summary>
        /// 發動事件，所需的計數
        /// </summary>
        public int NeedCounter;
        /// <summary>
        /// 事件管理器
        /// </summary>
        protected EventManager EventManager => EventManager.Instance;

        #region SetUp

        public PowerBase(){
            SubscribeAllEvent();
        }

        /// <summary>
        /// 訂閱所有事件
        /// </summary>
        protected void SubscribeAllEvent()
        {
            if (EventManager != null)
            {
                EventManager.onAttacked += OnAttacked;
                EventManager.OnQuestioningModeEnd += OnQuestioningModeEnd;
                EventManager.OnAnswer += OnAnswer;
                EventManager.OnAnswerCorrect += OnAnswerCorrect;
                EventManager.OnAnswerWrong += OnAnswerWrong;
            }
        }

        /// <summary>
        /// 取消訂閱所以事件
        /// </summary>
        protected void UnSubscribeAllEvent()
        {
            if (EventManager != null)
            {
                EventManager.onAttacked -= OnAttacked;
                EventManager.OnQuestioningModeEnd -= OnQuestioningModeEnd;
                EventManager.OnAnswer -= OnAnswer;
                EventManager.OnAnswerCorrect -= OnAnswerCorrect;
                EventManager.OnAnswerWrong -= OnAnswerWrong;
            }
        }
        
        #endregion
        
        

        #region 能力控制
        /// <summary>
        /// 增加能力數值
        /// </summary>
        /// <param name="stackAmount"></param>
        public virtual void StackPower(int stackAmount)
        {
            if (IsActive)
            {
                Value += stackAmount;
                Owner?.CharacterStats.OnPowerChanged.Invoke(PowerType, Value);
            }
            else
            {
                Value = stackAmount;
                IsActive = true;
                Owner?.CharacterStats.OnPowerApplied.Invoke(PowerType, Value);
            }
        }
        
        /// <summary>
        /// 降低能力數值
        /// </summary>
        /// <param name="reduceAmount"></param>
        public virtual void ReducePower(int reduceAmount)
        {
            Value -= reduceAmount;
            //Check status
            if (Value <= 0)
            {
                if (CanNegativeStack)
                {
                    if (Value == 0 && !IsPermanent)
                        ClearPower();
                }
                else
                {
                    if (!IsPermanent)
                        ClearPower();
                }
            }
            Owner.CharacterStats.OnPowerChanged.Invoke(PowerType, Value);
        }
        
        /// <summary>
        /// 清除能力
        /// </summary>
        public void ClearPower()
        {
            IsActive = false;
            Value = 0;
            Owner.CharacterStats.PowerDict.Remove(PowerType);
            Owner.CharacterStats.OnPowerCleared.Invoke(PowerType);
            UnSubscribeAllEvent();
        }

        #endregion

        #region 遊戲流程

        /// <summary>
        /// 回合開始時，觸發的方法
        /// </summary>
        public virtual void OnTurnStarted()
        {
            //One turn only statuses
            if (ClearAtNextTurn)
            {
                ClearPower();
                return;
            }
            
            if (DecreaseOverTurn) 
                ReducePower(1);
            
            //Check status
            if (Value <= 0)
            {
                if (CanNegativeStack)
                {
                    if (Value == 0 && !IsPermanent)
                        ClearPower();
                }
                else
                {
                    if (!IsPermanent)
                        ClearPower();
                }
            }
            
            
        }

        #endregion
        
        #region 戰鬥計算
        /// <summary>
        /// 受到傷害時，對傷害的加成
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual float AtDamageReceive(float damage)
        {
            return damage;
        }

        /// <summary>
        /// 給予對方傷害時，對傷害的加成
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual float AtDamageGive(float damage)
        {
            return damage;
        }
        
        /// <summary>
        /// 賦予格檔時，對格檔的加乘
        /// </summary>
        /// <param name="blockAmount"></param>
        /// <returns></returns>
        public virtual float ModifyBlock(float blockAmount) {
            return blockAmount;
        }

        /// <summary>
        /// 賦予格檔時，對格檔的加乘(最後觸發)
        /// </summary>
        /// <param name="blockAmount"></param>
        /// <returns></returns>
        public virtual float ModifyBlockLast(float blockAmount) {
            return blockAmount;
        }

        #endregion

        
        #region 事件觸發的方法
        /// <summary>
        /// 當能力改變時
        /// </summary>
        protected virtual void OnPowerChange(){}
        protected virtual void AtStartOfTurn(){}
        /// <summary>
        /// 受到攻擊時，觸發的方法
        /// </summary>
        /// <param name="info"></param>
        /// <param name="damageAmount"></param>
        protected virtual void OnAttacked(DamageInfo info, int damageAmount){}
        /// <summary>
        /// 開始問答模式時，觸發的方法
        /// </summary>
        protected virtual void OnQuestioningModeStart(){}
        /// <summary>
        /// 回答問題時，觸發的方法
        /// </summary>
        protected virtual void OnAnswer(){}
        /// <summary>
        /// 答對問題時，觸發的方法
        /// </summary>
        protected virtual void OnAnswerCorrect(){}
        /// <summary>
        /// 答錯問題時，觸發的方法
        /// </summary>
        protected virtual void OnAnswerWrong(){}
        /// <summary>
        /// 結束問答模式時，觸發的方法
        /// </summary>
        /// <param name="correctCount"></param>
        protected virtual void OnQuestioningModeEnd(int correctCount){}
        
        #endregion
    }
}