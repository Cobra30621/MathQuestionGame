using NueGames.CharacterAbility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Characters
{
    [CreateAssetMenu(fileName = "Ally Character Data ",menuName = "NueDeck/Characters/Ally",order = 0)]
    public class AllyCharacterData : CharacterDataBase
    {
        [InlineEditor()]
        public CharacterSkill CharacterSkill;
    }
}