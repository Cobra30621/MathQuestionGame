using NueGames.Action.MathAction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.CharacterAbility
{
    [CreateAssetMenu(fileName = "Character Ability", menuName = "NueDeck/CharacterAbility", order = 0)]
    public class CharacterAbility : SerializedScriptableObject
    {
        public string abilityName;
        public string abilityDescription;

        public int enablePlayCount;
        public QuestionActionParameters QuestionActionParameters;
    }
}