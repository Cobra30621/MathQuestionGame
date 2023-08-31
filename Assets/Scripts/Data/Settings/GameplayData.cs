using System.Collections.Generic;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Collection.RewardData;
using NueGames.Relic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Settings
{
    [CreateAssetMenu(fileName = "Gameplay Data", menuName = "NueDeck/Settings/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        [FoldoutGroup("基礎設定")]
        [PropertyTooltip("初始抽牌數量")]
        [SerializeField] private int drawCount = 4;
        
        [FoldoutGroup("基礎設定")]
        [PropertyTooltip("最大魔力")]
        [SerializeField] private int maxMana = 3;
        
        [FoldoutGroup("基礎設定")]
        [PropertyTooltip("最大手牌數量")]
        [SerializeField] private int maxCardOnHand;
        
        [InlineEditor]
        [FoldoutGroup("卡牌")]
        [PropertyTooltip("初始卡牌")]
        [SerializeField] private DeckData initalDeck;
        
        [InlineEditor]
        [FoldoutGroup("卡牌")]
        [PropertyTooltip("獎賞的卡牌")]
        [SerializeField] private CardRewardData cardRewardData;
        
        [FoldoutGroup("卡牌")]
        [PropertyTooltip("卡牌的 GameObject")]
        [SerializeField] private CardBase cardPrefab;

        [FoldoutGroup("玩家")] 
        [PropertyTooltip("玩家資料")] [InlineEditor()] [SerializeField]
        private AllyData initialAllyDataData;
        
        [FoldoutGroup("玩家")]
        [PropertyTooltip("玩家初始遺物")]
        [SerializeField] private List<RelicName> initialRelic;
        
        #region Encapsulation
        public int DrawCount => drawCount;
        public int MaxMana => maxMana;
        public AllyData InitialAllyData => initialAllyDataData;
        public DeckData InitalDeck => initalDeck;
        public CardRewardData CardRewardData => cardRewardData;
        public int MaxCardOnHand => maxCardOnHand;
        public CardBase CardPrefab => cardPrefab;
        public List<RelicName> InitialRelic => initialRelic;

        #endregion
    }
}