using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using Card.Data;
using Characters.Enemy.Data;
using Combat;
using Effect;
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

        // 這次要驗證的資料（正式或測試）
        private SkillData _skillData;
        private EnemyIntentionData _enemyIntentionData;
        private List<EnemyData> _enemies;
        private CardLevelData _cardLevelData;

        /// <summary>
        /// Validates the data from Google Sheets.
        /// </summary>
        public void ValidateSheet(SheetSource source = SheetSource.Official)
        {
            var isTest = source == SheetSource.Test;
            _skillData = isTest ? getter.testSkillData : getter.officialSkillData;
            _enemyIntentionData = isTest ? getter.testEnemyIntentionData : getter.officialEnemyIntentionData;
            _enemies = isTest ? getter.testEnemyData.GetAllData() : getter.officialEnemyData.GetAllData();
            _cardLevelData = isTest ? getter.testCardLevelData : getter.officialCardLevelData;

            ValidateEnemySkillData();
            ValidateEnemyData();
            ValidateSkillData();

            ValidateCardLevelData();
            ValidateCardData();

            Debug.Log($"{(isTest ? "測試" : "正式")}資料驗證完成");
        }

        private void ValidateCardLevelData()
        {
            foreach (var cardLevelInfo in _cardLevelData.GetAllCardInfo())
            {
                foreach (var skillID in cardLevelInfo.skillIDs)
                {
                    _skillData.GetSkillInfo(skillID, $"CardLevelData {cardLevelInfo.ID}: ");
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
            var groupIds = _cardLevelData.GetGroupIds();

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
            foreach (var enemyData in _enemies)
            {
                foreach (var enemySkillID in enemyData.enemyIntentionIDs)
                {
                    _enemyIntentionData.GetEnemyIntentionInfo(
                        enemySkillID, $"Enemy {enemyData.ID}: ");
                }

                foreach (var enemySkillID in enemyData.startBattleIntentionIDs)
                {
                    _enemyIntentionData.GetEnemyIntentionInfo(
                        enemySkillID, $"Enemy {enemyData.ID}: ");
                }
                
                getter.enemyPrefabData.GetPrefab(enemyData.Prefab, $"Enemy {enemyData.ID}: ");
            }
        }

        private void ValidateEnemySkillData()
        {
            // Validates skill IDs and intentions for enemy skills.
            foreach (var enemySkillData in _enemyIntentionData.EnemyIntentions)
            {
                foreach (var skillID in enemySkillData.skillIDs)
                {
                    _skillData.GetSkillInfo(skillID, $"EnemySkill {enemySkillData.SkillID}: ");
                }

                getter.intentionData.GetIntention(enemySkillData.Intention, $"EnemySkill {enemySkillData.SkillID}: ");
            }
        }

        private void ValidateSkillData()
        {
            // Validates effect IDs and targets for skills.
            foreach (var skillInfo in _skillData.GetAllSkillInfos())
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