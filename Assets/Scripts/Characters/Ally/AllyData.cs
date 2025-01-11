using System.Collections.Generic;
using Card.Data;
using NueGames.Relic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters.Ally
{
    [CreateAssetMenu(fileName = "Ally Character Data ",menuName = "Characters/Ally",order = 0)]
    public class AllyData : SerializedScriptableObject, ISerializeReferenceByAssetGuid
    {
        [SerializeField] protected string characterName;
        [SerializeField] [TextArea] protected string characterDescription;
        [SerializeField] protected int maxHealth;

        public string CharacterName => characterName;

        public string CharacterDescription => characterDescription;

        public int MaxHealth => maxHealth;
        
        [LabelText("玩家 Prefab")]
        public Ally prefab;
      
        [LabelText("玩家初始卡牌")]
        [InlineEditor]
        public DeckData InitialDeck;
        
        [LabelText("玩家獎勵卡組")]
        public DeckData CardRewardData;
        
        [LabelText("玩家初始遺物")]
        public List<RelicName> initialRelic;
        

        [LabelText("大圖像")]
        public Sprite Sprite;

        [LabelText("選擇的小 Icon")]
        public Sprite Icon;
    }
}