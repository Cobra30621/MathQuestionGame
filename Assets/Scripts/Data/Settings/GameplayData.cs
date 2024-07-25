using System.Collections.Generic;
using Card.Data;
using Map;
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
    public class GameplayData : ScriptableObject, ISerializeReferenceByAssetGuid
    {
        [FoldoutGroup("基礎設定")]
        [LabelText("初始抽牌數量")]
        [SerializeField] private int drawCount = 4;
        
        [FoldoutGroup("基礎設定")]
        [LabelText("最大魔力")]
        [SerializeField] private int maxMana = 3;
        
        [FoldoutGroup("基礎設定")]
        [LabelText("最大手牌數量")]
        [SerializeField] private int maxCardOnHand;
        
        [FoldoutGroup("基礎設定")]
        [LabelText("初始金錢")]
        [SerializeField] private int initMoney;
        
        [InlineEditor]
        [FoldoutGroup("卡牌")]
        [LabelText("初始卡組")]
        [SerializeField] private DeckData initalDeck;
        
        
        
        [InlineEditor]
        [FoldoutGroup("卡牌")]
        [LabelText("獎賞的卡牌")]
        [SerializeField] private CardRewardData cardRewardData;
        
        [FoldoutGroup("卡牌")]
        [LabelText("卡牌的 Prefab")]
        [SerializeField] private BattleCard battleCardPrefab;

        [FoldoutGroup("玩家")] 
        [LabelText("玩家資料")] [InlineEditor()] [SerializeField]
        private AllyData initialAllyDataData;
        
        [FoldoutGroup("玩家")]
        [LabelText("玩家初始遺物")]
        [SerializeField] private List<RelicName> initialRelic;

        [FoldoutGroup("地圖")]
        [LabelText("地圖")]
        [SerializeField] private MapConfig[] _mapConfigs;
        
        #region Encapsulation
        public int DrawCount => drawCount;
        public int MaxMana => maxMana;
        public AllyData InitialAllyData => initialAllyDataData;
        public DeckData InitalDeck => initalDeck;
        public CardRewardData CardRewardData => cardRewardData;
        public int MaxCardOnHand => maxCardOnHand;
        public BattleCard BattleCardPrefab => battleCardPrefab;
        public List<RelicName> InitialRelic => initialRelic;
        public MapConfig[] MapConfigs => _mapConfigs;
        public int InitMoney => initMoney;

        #endregion
    }
}