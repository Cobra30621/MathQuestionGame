using System;
using NueGames.Data.Characters;
using NueGames.Interfaces;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Characters
{
    public abstract class AllyBase : CharacterBase,IAlly
    {
        [Header("Ally Base Settings")]
        [SerializeField] private AllyCanvas allyCanvas;
        [SerializeField] private AllyCharacterData allyCharacterData;
        public AllyCanvas AllyCanvas => allyCanvas;
        public AllyCharacterData AllyCharacterData => allyCharacterData;
        
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            allyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(allyCharacterData.MaxHealth, this);
            CharacterStats.SetCharacterCanvasEvent(allyCanvas);

            if (!GameManager)
                throw new Exception("There is no GameManager");
            
            var data = GameManager.PersistentGameplayData.AllyHealthDataList.Find(x =>
                x.CharacterId == AllyCharacterData.CharacterID);
            
            if (data != null)
            {
                CharacterStats.CurrentHealth = data.CurrentHealth;
                CharacterStats.MaxHealth = data.MaxHealth;
            }
            else
            {
                GameManager.PersistentGameplayData.SetAllyHealthData(AllyCharacterData.CharacterID,CharacterStats.CurrentHealth,CharacterStats.MaxHealth);
            }
            
            CharacterStats.OnDeath += OnDeath;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
            
            if (CombatManager != null)
                CombatManager.OnRoundEnd += CharacterStats.HandleAllPowerOnRoundEnd;
        }
        
        protected override void OnDeath()
        {
            base.OnDeath();
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