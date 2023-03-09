using NueGames.NueDeck.Scripts.Characters;

namespace NueGames.NueDeck.Scripts.Combat
{
    public class DamageInfo
    {
        public CharacterBase SelfCharacter;
        public int Value;

        public DamageInfo(int value, CharacterBase selfCharacter)
        {
            Value = value;
            SelfCharacter = selfCharacter;
        }
    }
}