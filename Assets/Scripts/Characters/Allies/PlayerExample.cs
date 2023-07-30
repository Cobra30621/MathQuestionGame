using NueGames.Managers;
using UnityEngine;

namespace NueGames.Characters.Allies
{
    public class PlayerExample : AllyBase
    {
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            if (UIManager != null)
                OnHealthChanged += UIManager.InformationCanvas.SetHealthText;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
        }
    }
}