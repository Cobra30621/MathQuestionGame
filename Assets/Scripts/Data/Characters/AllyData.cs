using NueGames.CharacterAbility;
using NueGames.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Characters
{
    [CreateAssetMenu(fileName = "Ally Character Data ",menuName = "Characters/Ally",order = 0)]
    public class AllyData : CharacterDataBase, ISerializeReferenceByAssetGuid
    {
        public AllyBase prefab;
        [InlineEditor()]
        public CharacterSkill CharacterSkill;
    }
}