﻿using System;
using System.Collections.Generic;
using System.Linq;
using Card.Data;
using Combat;
using Effect.Parameters;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheets
{
    /// <summary>
    /// Validates the data from Google Sheets for enemies and enemy skills.
    /// </summary>
    public class SheetDataValidator : MonoBehaviour
    {
        [Required]
        public SheetDataGetter getter;

        /// <summary>
        /// Validates the data from Google Sheets.
        /// </summary>
        public void ValidateSheet()
        {
            ValidateEnemySkillData();
            ValidateEnemyData();
            ValidateSkillData();

            ValidateCardLevelData();
            ValidateCardData();

            Debug.Log("驗證完成");
        }

        private void ValidateCardLevelData()
        {
            foreach (var cardLevelInfo in getter.cardLevelData.GetAllCardInfo())
            {
                foreach (var skillID in cardLevelInfo.skillIDs)
                {
                    getter.skillData.GetSkillInfo(skillID, $"CardLevelData {cardLevelInfo.ID}: ");
                }
                
                if (!Enum.IsDefined(typeof(AllyClassType), cardLevelInfo.Class))
                {
                    Debug.LogError($"Invalid {nameof(AllyClassType)} " +
                                   $"'{cardLevelInfo.Class}' in CardLevelInfo '{cardLevelInfo.ID}'");
                }
            }
        }

        private void ValidateCardData()
        {
            var groupIds = getter.cardLevelData.GetGroupIds();

            var cardList = new List<CardData>();
            cardList.AddRange(getter.officialDeck.CardList);
            cardList.AddRange(getter.developDeck.CardList);
            
            
            foreach (var groupId in groupIds)
            {
                var firstOrDefault =  cardList.FirstOrDefault(x => x.CardId == groupId);
                if (firstOrDefault == null)
                {
                    Debug.LogError($"未找到卡片組 {groupId} 對應的 CardData");
                }
            }
        }

        private void ValidateEnemyData()
        {
            // Validates enemy skill IDs and checks for prefabs.
            foreach (var enemyData in getter.enemyData.GetAllData())
            {
                foreach (var enemySkillID in enemyData.enemyIntentionIDs)
                {
                    getter.enemyIntentionData.GetEnemyIntentionInfo(
                        enemySkillID, $"Enemy {enemyData.ID}: ");
                }

                foreach (var enemySkillID in enemyData.startBattleIntentionIDs)
                {
                    getter.enemyIntentionData.GetEnemyIntentionInfo(
                        enemySkillID, $"Enemy {enemyData.ID}: ");
                }
                
                getter.enemyPrefabData.GetPrefab(enemyData.Prefab, $"Enemy {enemyData.ID}: ");
            }
        }

        private void ValidateEnemySkillData()
        {
            // Validates skill IDs and intentions for enemy skills.
            foreach (var enemySkillData in getter.enemyIntentionData.EnemyIntentions)
            {
                foreach (var skillID in enemySkillData.skillIDs)
                {
                    getter.skillData.GetSkillInfo(skillID, $"EnemySkill {enemySkillData.SkillID}: ");
                }

                getter.intentionData.GetIntention(enemySkillData.Intention, $"EnemySkill {enemySkillData.SkillID}: ");
            }
        }

        private void ValidateSkillData()
        {
            // Validates effect IDs and targets for skills.
            foreach (var skillInfo in getter.skillData.GetAllSkillInfos())
            {
                if (!Enum.IsDefined(typeof(EffectName), skillInfo.EffectID))
                {
                    Debug.LogError($"Invalid SkillEffectID '{skillInfo.EffectID}' in Skill '{skillInfo.SkillID}'");
                }

                if (!Enum.IsDefined(typeof(ActionTargetType), skillInfo.Target))
                {
                    Debug.LogError($"Invalid Target '{skillInfo.Target}' in Skill '{skillInfo.SkillID}'");
                }
            }
        }
    }
}