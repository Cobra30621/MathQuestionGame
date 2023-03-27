using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Power
{
    public abstract class PowerBase
    {
        public abstract PowerType PowerType  { get;}
        public int Value;
        public bool IsActive;
        public bool DecreaseOverTurn;// If true, decrease on turn end
        public bool IsPermanent; // If true, status can not be cleared during combat
        public bool CanNegativeStack;
        public bool ClearAtNextTurn;
        public CharacterBase Owner;

        public int Counter;
        public int NeedCounter;
        
        protected EventManager EventManager => EventManager.Instance;

        #region SetUp

        public PowerBase(){
            SubscribeAllEvent();
        }

        protected void SubscribeAllEvent()
        {
            EventManager.onAttacked += OnAttacked;
            EventManager.OnQuestioningModeEnd += OnQuestioningModeEnd;
            EventManager.OnAnswer += OnAnswer;
            EventManager.OnAnswerCorrect += OnAnswerCorrect;
            EventManager.OnAnswerWrong += OnAnswerWrong;
        }

        protected void UnSubscribeAllEvent()
        {
            EventManager.onAttacked -= OnAttacked;
            EventManager.OnQuestioningModeEnd -= OnQuestioningModeEnd;
            EventManager.OnAnswer -= OnAnswer;
            EventManager.OnAnswerCorrect -= OnAnswerCorrect;
            EventManager.OnAnswerWrong -= OnAnswerWrong;
        }
        
        #endregion
        
        

        #region Power Control
        public virtual void StackPower(int stackAmount)
        {
            if (IsActive)
            {
                Value += stackAmount;
                Owner.CharacterStats.OnPowerChanged.Invoke(PowerType, Value);
            }
            else
            {
                Value = stackAmount;
                IsActive = true;
                Owner.CharacterStats.OnPowerApplied.Invoke(PowerType, Value);
            }
        }
        
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
        
        public void ClearPower()
        {
            IsActive = false;
            Value = 0;
            Owner.CharacterStats.PowerDict.Remove(PowerType);
            Owner.CharacterStats.OnPowerCleared.Invoke(PowerType);
            UnSubscribeAllEvent();
        }

        #endregion

        #region Game Process

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
        
        #region Combat Calculate
        public virtual float AtDamageReceive(float damage)
        {
            return damage;
        }

        public virtual float AtDamageGive(float damage)
        {
            return damage;
        }
        
        public virtual float ModifyBlock(float blockAmount) {
            return blockAmount;
        }

        public virtual float ModifyBlockLast(float blockAmount) {
            return blockAmount;
        }

        #endregion

        
        #region Event
        protected virtual void OnPowerChange(){}
        protected virtual void AtStartOfTurn(){}
        protected virtual void OnAttacked(DamageInfo info, int damageAmount){}
        
        protected virtual void OnQuestioningModeStart(){}
        
        protected virtual void OnAnswer(){}
        protected virtual void OnAnswerCorrect(){}
        protected virtual void OnAnswerWrong(){}
        protected virtual void OnQuestioningModeEnd(int correctCount){}
        
        #endregion
    }
}