using NueGames.Data.Collection;
using NueGames.Enums;
using Sirenix.OdinInspector;

namespace Action.Parameters
{
    public class CardTransfer
    {
        [InfoBox("用於與卡組相關的遊戲行為")]
        [FoldoutGroup("卡組參數")]
        [PropertyTooltip("起始的卡組")]
        public PileType SourcePile;
        
        [FoldoutGroup("卡組參數")]
        [PropertyTooltip("目標的卡組")]
        public PileType TargetPile;
        
        [FoldoutGroup("卡組參數")]
        [PropertyTooltip("目標卡牌")]
        public CardData TargetCardData;
    }
}