using NueGames.Card;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Parameters;
using NueGames.Relic;

namespace Action.Parameters
{
    /// <summary>
    /// 行為來源參數
    /// </summary>
    public class ActionSource
    {
        /// <summary>
        /// 行為來源
        /// </summary>
        public SourceType SourceType;
        /// <summary>
        /// 源自哪一個能力(能力才需要)
        /// </summary>
        public PowerType SourcePower;
        /// <summary>
        /// 源自哪一個遺物(遺物才需要)
        /// </summary>
        public RelicType SourceRelic;
        /// <summary>
        /// 源自哪個角色
        /// </summary>
        public CharacterBase SourceCharacter;
        /// <summary>
        /// 源自於哪張卡片
        /// </summary>
        public CardBase SourceCard;

        public bool IsFromPower(PowerType checkPower)
        {
            return SourceType == SourceType.Power && SourcePower == checkPower;
        }
        
    }
}