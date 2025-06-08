using System;
using Combat;
using Effect.Parameters;
using UI;

namespace Characters.Ally
{
    public class Ally : CharacterBase
    {
        private AllyData _allyData;
        
        private CharacterHandler _characterHandler;

        
        public void BuildCharacter(AllyData allyData, CharacterHandler characterHandler)
        {
            _characterHandler = characterHandler;
            _allyData = allyData;
            
            SetUpFeedbackDict();
            _characterCanvas.InitCanvas();
            
            
            var data = GameManager.AllyHealthHandler.GetAllyHealthData();
            
            CharacterStats = new CharacterStats(data.CurrentHealth, 
                data.MaxHealth, this, _characterCanvas);
            
            SubscribeEvent();
        }
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            _characterHandler.OnAllyDeath(this);

            Destroy(gameObject);
        }

        protected override void SubscribeEvent()
        {
            base.SubscribeEvent();
            OnHealthChanged +=  UIManager.Instance.InformationCanvas.SetHealthText;
        }

        protected override void UnsubscribeEvent()
        {
            base.UnsubscribeEvent();
            if ( UIManager.Instance != null)
                OnHealthChanged -=  UIManager.Instance.InformationCanvas.SetHealthText;
        }
    }

    [Serializable]
    public class AllyHealthData
    {
        public int MaxHealth;

        public int CurrentHealth;

        public AllyHealthData(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }


        public void SetHealth(int afterHealHp, int healthDataMaxHealth)
        {
            CurrentHealth = afterHealHp;
            MaxHealth = healthDataMaxHealth;
        }
    }
}