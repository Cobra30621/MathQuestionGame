using Combat.Card;
using NueGames.Card;
using Save;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Settings
{
    [CreateAssetMenu(fileName = "Gameplay Data", menuName = "NueDeck/Settings/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        [FoldoutGroup("基礎設定")]
        [LabelText("初始抽牌數量")]
        [SerializeField] private int drawCount = 6;
        
        [FoldoutGroup("基礎設定")]
        [LabelText("最大魔力")]
        [SerializeField] private int maxMana = 3;
        
        [FoldoutGroup("基礎設定")]
        [LabelText("最大手牌數量")]
        [SerializeField] private int maxCardOnHand;
        
        [FoldoutGroup("基礎設定")]
        [LabelText("初始金錢")]
        [SerializeField] private int initMoney;
        
        [FoldoutGroup("基礎設定")]
        [LabelText("初始寶石")]
        [SerializeField] private int initStone;
        

        [FoldoutGroup("基礎設定")]
        [LabelText("卡牌的 Prefab")]
        [SerializeField] private BattleCard battleCardPrefab;

        #region Encapsulation
        public int DrawCount => drawCount;
        public int MaxMana => maxMana;
        public int MaxCardOnHand
        {
            get => maxCardOnHand;
            set => maxCardOnHand = value;
        }
        public BattleCard BattleCardPrefab => battleCardPrefab;
        public int InitMoney => initMoney;
        public int InitStone => initStone;

        #endregion
    }
}