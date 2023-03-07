using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;

namespace NueGames.NueDeck.Scripts.Power
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
        
        protected EventManager EventManager => EventManager.Instance;

        #region SetUp

        public PowerBase(){
            SubscribeAllEvent();
        }

        protected void SubscribeAllEvent()
        {
            EventManager.onAttacked += OnAttacked;
        }
        
        #endregion

        #region Power Control
        public virtual void StackPower(int stackAmount)
        {
            if (IsActive)
            {
                Value += stackAmount;
            }
            else
            {
                Value = stackAmount;
                IsActive = true;
            }
        }
        
        public virtual void ReducePower(int reduceAmount) {
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
                    if (IsPermanent)
                        ClearPower();
                }
            }
        }
        
        public void ClearPower()
        {
            IsActive = false;
            Value = 0;
            EventManager.OnPowerCleared.Invoke(PowerType);
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
        protected virtual void AtStartOfTurn(){}
        protected virtual void OnAttacked(){}
        
        #endregion
    }
}