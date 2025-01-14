using Characters;
using Combat.Card;
using Power;
using Relic.Data;

namespace Effect.Parameters
{
    /// <summary>
    /// 行為來源參數
    /// </summary>
    public class EffectSource
    {
        /// <summary>
        /// 行為來源
        /// </summary>
        public SourceType SourceType;
        /// <summary>
        /// 源自哪一個能力(能力才需要)
        /// </summary>
        public PowerName SourcePower;
        /// <summary>
        /// 源自哪一個遺物(遺物才需要)
        /// </summary>
        public RelicName SourceRelic;
        /// <summary>
        /// 源自哪個角色
        /// </summary>
        public CharacterBase SourceCharacter;
        /// <summary>
        /// 源自於哪張卡片
        /// </summary>
        public BattleCard SourceBattleCard;

        public bool IsFromPower(PowerName checkPower)
        {
            return SourceType == SourceType.Power && SourcePower == checkPower;
        }


        public override string ToString()
        {
            switch (SourceType)
            {
                case SourceType.Power:
                    return $"來自能力 {SourcePower}";
                case SourceType.Relic:
                    return $"來自遺物 {SourceRelic}";
                case SourceType.Enemy:
                    return $"來自敵人 {SourceCharacter.name}";
                case SourceType.Card:
                    return $"來自卡片 {SourceBattleCard.name}";
                default:
                    return "未指定來源";
            }
        }
    }
}