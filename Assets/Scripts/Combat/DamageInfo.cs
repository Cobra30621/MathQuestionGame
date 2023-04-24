using NueGames.Characters;

namespace NueGames.Combat
{
    public class DamageInfo
    {
        public CharacterBase Self;
        public CharacterBase Target;
        public int Value;
        /// <summary>
        /// 固定傷害，不受狀態影響
        /// </summary>
        public bool FixDamage;

        public DamageInfo(int value, CharacterBase self)
        {
            Value = value;
            Self = self;
        }
        
        public DamageInfo()
        {
        }
    }
}