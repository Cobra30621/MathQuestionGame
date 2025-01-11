using Card.Data;
using Combat.Card;
using NueGames.Enums;
using Sirenix.OdinInspector;

namespace Action.Parameters
{
    /// <summary>
    /// 卡牌轉移參數
    /// </summary>
    public class CardTransfer
    {
        public int CardCount = 1;
        
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