using NueGames.NueDeck.Scripts.Characters;

namespace NueGames.NueDeck.Scripts.Combat
{
    public class DamageInfo
    {
        public CharacterBase SelfCharacter;
        public int Value;
        public int AdditionalValue;

        public DamageInfo(int value, CharacterBase selfCharacter)
        {
            Value = value;
            SelfCharacter = selfCharacter;
        }
        
        public DamageInfo(int value, int additionalValue, CharacterBase selfCharacter)
        {
            Value = value;
            AdditionalValue = additionalValue;
            SelfCharacter = selfCharacter;
        }
    }
}