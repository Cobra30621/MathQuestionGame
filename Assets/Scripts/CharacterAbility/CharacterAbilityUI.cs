using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.CharacterAbility
{
    public class CharacterAbilityUI : SerializedMonoBehaviour
    {
        [SerializeField] private Text abilityName, description;
        
        
        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            abilityName.text = CharacterAbilityManager.Instance.CharacterAbility.abilityName;
            description.text = CharacterAbilityManager.Instance.CharacterAbility.abilityDescription;
        }

        public void PlayCharacterAbility()
        {
            CharacterAbilityManager.Instance.PlayAbility();
        }
    }
}