using System;
using Action.Parameters;
using Combat;
using NueGames.CharacterAbility;
using NueGames.Combat;
using NueGames.Data.Characters;
using NueGames.Managers;
using NueGames.Parameters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Characters
{
    public abstract class AllyBase : CharacterBase
    {
        [Header("Ally Base Settings")]
        [SerializeField] private AllyCanvas allyCanvas;
        private AllyData _allyData;
        public AllyCanvas AllyCanvas => allyCanvas;
        
        private CharacterHandler _characterHandler;

        
        
        public void BuildCharacter(AllyData allyData, CharacterHandler characterHandler)
        {
            _characterHandler = characterHandler;
            _allyData = allyData;
            
            SetUpFeedbackDict();
            allyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(_allyData.MaxHealth, this, allyCanvas);
            
            var data = GameManager.PlayerData.AllyHealthData;
            
            if (data != null)
            {
                CharacterStats.CurrentHealth = data.CurrentHealth;
                CharacterStats.MaxHealth = data.MaxHealth;
            }
            else
            {
                GameManager.PlayerData.SetHealth(CharacterStats.CurrentHealth,CharacterStats.MaxHealth);
            }
            
            OnDeath += OnDeathAction;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
            
            if (CombatManager != null)
                CombatManager.OnRoundEnd += CharacterStats.HandleAllPowerOnRoundEnd;
            
            if (UIManager != null)
                OnHealthChanged += UIManager.InformationCanvas.SetHealthText;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
        }
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            if (CombatManager != null)
            {
                CombatManager.OnRoundEnd -= CharacterStats.HandleAllPowerOnRoundEnd;
                _characterHandler.OnAllyDeath(this);
            }

            Destroy(gameObject);
        }
    }

    [Serializable]
    public class AllyHealthData
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        
        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }
    }
}