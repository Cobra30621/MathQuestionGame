using NueGames.Action.MathAction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.CharacterAbility
{
    [CreateAssetMenu(fileName = "Character Ability", menuName = "NueDeck/CharacterAbility", order = 0)]
    public class CharacterSkill : SerializedScriptableObject
    {
        public string abilityName;
        public string abilityDescription;

        public int skillCount;
        public QuestionActionParameters QuestionActionParameters;
    }
}