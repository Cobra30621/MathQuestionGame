using System;
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
        public SheetDataGetter getter;

        /// <summary>
        /// Validates the data from Google Sheets.
        /// </summary>
        [Button("驗證")]
        public void ValidateSheet()
        {
            ValidateEnemySkillData();
            ValidateEnemyData();
            ValidateSkillData();

            Debug.Log("驗證完成");
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
            foreach (var skillInfo in getter.skillData.GetSkillInfos())
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