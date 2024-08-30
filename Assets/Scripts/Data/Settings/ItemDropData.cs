using System;
using Map;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEngine.Random;

namespace Data.Settings
{
    public class ItemDropData : ScriptableObject
    {
        [LabelText("小怪掉錢")]
        public int minorEnemyDropMoney;
        [LabelText("精英怪掉錢")]
        public int eliteEnemyDropMoney;
        [LabelText("Boss 掉錢")]
        public int bossDropMoney;
        [LabelText("寶箱 掉錢")]
        public int treasureRoomDropMoney;

        [LabelText("金錢隨機浮動")] public float randomMoneyRange;
        
        public int questionDropMoney;
        public int questionDropStone;

        public int GetNodeDropMoney(NodeType nodeType)
        {
            var moneyDropRate = GameManager.Instance.GetMoneyDropRate();
            
            float rate = Range(1f - randomMoneyRange, 1f + randomMoneyRange) * moneyDropRate;
            
            switch (nodeType)
            {
                case NodeType.MinorEnemy:
                    return (int) Math.Floor(minorEnemyDropMoney * rate);
                case NodeType.EliteEnemy:
                    return (int) Math.Floor(eliteEnemyDropMoney * rate);
                case NodeType.Boss:
                    return (int) Math.Floor(bossDropMoney * rate);
                case NodeType.Treasure:
                    return (int) Math.Floor(treasureRoomDropMoney * rate);
                default:
                    Debug.LogError("nodeType: " + nodeType + " not supported drop Money");
                    return 0;
            }
        }
    }
}