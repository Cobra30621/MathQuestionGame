using System;
using System.Linq;
using Action;
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

            foreach (var groupId in groupIds)
            {
                var firstOrDefault = getter.saveDeck.CardList.FirstOrDefault(x => x.CardId == groupId);
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
                foreach (var enemySkillID in enemyData.enemySkillIDs)
                {
                    getter.enemySkillData.GetEnemySkillData(
                        enemySkillID, $"Enemy {enemyData.ID}: ");
                }

                if (enemyData.StartBattleSkillID != "")
                {
                    getter.enemySkillData.GetEnemySkillData(enemyData.StartBattleSkillID, $"Enemy {enemyData.ID}: ");
                }

                getter.enemyPrefabData.GetPrefab(enemyData.Prefab, $"Enemy {enemyData.ID}: ");
            }
        }

        private void ValidateEnemySkillData()
        {
            // Validates skill IDs and intentions for enemy skills.
            foreach (var enemySkillData in getter.enemySkillData.EnemySkills)
            {
                foreach (var skillID in enemySkillData.skillIDs)
                {
                    getter.skillData.GetSkillInfo(skillID, $"EnemySkill {enemySkillData.ID}: ");
                }

                getter.intentionData.GetIntention(enemySkillData.Intention, $"EnemySkill {enemySkillData.ID}: ");
            }
        }

        private void ValidateSkillData()
        {
            // Validates effect IDs and targets for skills.
            foreach (var skillInfo in getter.skillData.GetAllSkillInfos())
            {
                if (!Enum.IsDefined(typeof(GameActionType), skillInfo.EffectID))
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