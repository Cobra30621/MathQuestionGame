using System;
using Action.Parameters;
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


        public void SetCharacterData(AllyData data)
        {
            _allyData = data;
        }

        public CharacterSkill GetCharacterSkill()
        {
            return _allyData.CharacterSkill;
        }
        
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            allyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(_allyData.MaxHealth, this);
            CharacterStats.SetCharacterCanvasEvent(allyCanvas);

            if (!GameManager)
                throw new Exception("There is no GameManager");
            
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
        }
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            if (CombatManager != null)
            {
                CombatManager.OnRoundEnd -= CharacterStats.HandleAllPowerOnRoundEnd;
                CombatManager.OnAllyDeath(this);
            }

            Destroy(gameObject);
        }
    }

    [Serializable]
    public class AllyHealthData
    {
        [SerializeField] private string characterId;
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

        public string CharacterId
        {
            get => characterId;
            set => characterId = value;
        }
    }
}