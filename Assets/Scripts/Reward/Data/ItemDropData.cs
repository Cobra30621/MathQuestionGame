using System;
using System.Collections.Generic;
using System.Linq;
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
        public RelicName GetRelicData(NodeType nodeType)
        {
            var currentRelics = GameManager.Instance.RelicManager.CurrentRelicDict.Keys.ToList();

            List<RelicName> availableCommonRelics = commonRelics?
                .Where(r => !currentRelics.Contains(r)).ToList();
            var classRelics = GameManager.Instance.stageSelectedManager.RewardDropRelic();
            List<RelicName> availableClassRelics = classRelics?
                .Where(r => !currentRelics.Contains(r)).ToList();

            bool useCommonFirst = Range(0f, 1f) < commonRelicDropRate;
            RelicName relicName;

            if (useCommonFirst)
            {
                if (availableCommonRelics != null && availableCommonRelics.Count > 0)
                {
                    relicName = availableCommonRelics[Range(0, availableCommonRelics.Count)];
                }
                else if (availableClassRelics != null && availableClassRelics.Count > 0)
                {
                    relicName = availableClassRelics[Range(0, availableClassRelics.Count)];
                }
                else
                {
                    Debug.LogWarning("沒有可用的遺物（通用與職業都已擁有）");
                    return default;
                }
            }
            else
            {
                if (availableClassRelics != null && availableClassRelics.Count > 0)
                {
                    relicName = availableClassRelics[Range(0, availableClassRelics.Count)];
                }
                else if (availableCommonRelics != null && availableCommonRelics.Count > 0)
                {
                    relicName = availableCommonRelics[Range(0, availableCommonRelics.Count)];
                }
                else
                {
                    Debug.LogWarning("沒有可用的遺物（職業與通用都已擁有）");
                    return default;
                }
            }

            return relicName;
        }





        public int GetNodeDropStone(NodeType nodeType)
        {
            return questionDropStone;
        }
    }
}