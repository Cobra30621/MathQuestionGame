using System;
using Action.Parameters;
using Combat;

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
            CharacterStats = new CharacterStats(_allyData.MaxHealth, this, _characterCanvas);
            
            var data = GameManager.AllyHealthHandler.GetAllyHealthData();
            CharacterStats.CurrentHealth = data.CurrentHealth;
            CharacterStats.MaxHealth = data.MaxHealth;
            
            
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
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
            OnHealthChanged += UIManager.InformationCanvas.SetHealthText;
        }

        protected override void UnsubscribeEvent()
        {
            base.UnsubscribeEvent();
            OnHealthChanged -= UIManager.InformationCanvas.SetHealthText;
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