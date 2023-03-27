using NueGames.Characters;
using NueGames.Enums;

namespace NueGames.Interfaces
{
    public interface ICharacter
    {
        public CharacterBase GetCharacterBase();
        public CharacterType GetCharacterType();
    }
}