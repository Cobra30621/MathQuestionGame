using NueGames.NueDeck.Scripts.Characters;

namespace NueGames.NueDeck.Scripts.Combat
{
    public class DamageInfo
    {
        public CharacterBase SelfCharacter;
        public int Value;
        public int AddtionValue;

        public DamageInfo(int value, CharacterBase selfCharacter)
        {
            Value = value;
            SelfCharacter = selfCharacter;
        }
        
        public DamageInfo(int value, int addtionValue, CharacterBase selfCharacter)
        {
            Value = value;
            AddtionValue = addtionValue;
            SelfCharacter = selfCharacter;
        }
    }
}