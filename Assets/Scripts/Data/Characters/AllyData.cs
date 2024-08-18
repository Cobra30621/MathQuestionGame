using System.Collections.Generic;
using Card.Data;
using NueGames.CharacterAbility;
using NueGames.Characters;
using NueGames.Data.Collection.RewardData;
using NueGames.Relic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Characters
{
    [CreateAssetMenu(fileName = "Ally Character Data ",menuName = "Characters/Ally",order = 0)]
    public class AllyData : CharacterDataBase, ISerializeReferenceByAssetGuid
    {
        [LabelText("玩家 Prefab")]
        public AllyBase prefab;
      
        [LabelText("玩家初始卡牌")]
        public DeckData InitialDeck;
        
        [LabelText("玩家獎勵卡組")]
        public CardRewardData CardRewardData;
        
        [LabelText("玩家初始遺物")]
        public List<RelicName> initialRelic;
        

        [LabelText("大圖像")]
        public Sprite Sprite;

        [LabelText("選擇的小 Icon")]
        public Sprite Icon;
    }
}