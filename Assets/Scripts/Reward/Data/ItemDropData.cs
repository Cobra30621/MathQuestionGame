using System;
using System.Collections.Generic;
using Managers;
using Map;
using NueGames.Enums;
using Relic.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEngine.Random;

namespace Reward.Data
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
        
        [LabelText("答題掉錢")]
        public int questionDropMoney;
        [LabelText("答題寶石")]
        public int questionDropStone;

        [LabelText("通用遺物獎勵清單")] 
        public List<RelicName> commonRelics;

        [LabelText("通用遺物掉落機率")] 
        public float commonRelicDropRate;
        
        [LabelText("通用卡片掉落機率")] 
        public float commonCardDropRate;
        
        [Required]
        [LabelText("遺物資料")] public RelicsData relicsData;

        public int GetNodeDropMoney(NodeType nodeType)
        {
            float rate = Range(1f - randomMoneyRange, 1f + randomMoneyRange);
            
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
                case NodeType.Event:
                    return (int) Math.Floor(questionDropMoney * rate);
                default:
                    Debug.LogError("nodeType: " + nodeType + " not supported drop Money");
                    return 0;
            }
        }

        [Button]
        public (RelicName, RelicData) GetRelicData(NodeType nodeType)
        {
            var currentAllyClassType = GameManager.Instance.stageSelectedManager.CurrentAllyClassType();
            
            RelicName relicName;
            // 根據機率決定使用通用遺物還是職業特定遺物
            if (Range(0f, 1f) < commonRelicDropRate)
            {
                if (commonRelics == null || commonRelics.Count == 0)
                {
                    Debug.LogError("通用遺物清單為空");
                    return default;
                }
                relicName = commonRelics[Range(0, commonRelics.Count)];
            }
            else
            {
                var classRelics = GameManager.Instance.stageSelectedManager.RewardDropRelic();
                
                if (classRelics == null || classRelics.Count == 0)
                {
                    Debug.LogError($"職業 {currentAllyClassType} 的遺物清單為空");
                    return default;
                }
                
                relicName = classRelics[Range(0, classRelics.Count)];
            }

            var data = relicsData.GetRelicData(relicName);
            return (relicName, data);
        }

        public (RelicName, RelicData) GetRelicData(RelicName relicName)
        {
            var data = relicsData.GetRelicData(relicName);
            return (relicName, data);
        }

        public int GetNodeDropStone(NodeType nodeType)
        {
            return questionDropStone;
        }
    }
}