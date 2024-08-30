using Card;
using Card.Data;
using Enemy;
using Enemy.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sheets
{
    /// <summary>
    /// Retrieves data from various sheets for use in the game.
    /// </summary>
    public class SheetDataGetter : ScriptableObject
    {
        #region Sheets
        [Required] [InlineEditor] [LabelText("敵人")]
        public EnemyDataOverview enemyData;
        
        [Required] [InlineEditor] [LabelText("敵人技能")]
        public EnemySkillDataOverview enemySkillData;
        
        [Required] [InlineEditor] [LabelText("技能")]
        public SkillData skillData;
        
        [Required] [InlineEditor] [LabelText("卡片等級")]
        public CardLevelData cardLevelData;
        #endregion
        
        [Required] [InlineEditor] [LabelText("敵人 Prefab")]
        public EnemyPrefabData enemyPrefabData;
        
        [Required] [InlineEditor] [LabelText("意圖")]
        public IntentionData intentionData;
        
        [Required] [InlineEditor] [LabelText("需存檔用的卡片")]
        public DeckData saveDeck;

        /// <summary>
        /// Retrieves intention data based on the given ID.
        /// </summary>
        /// <param name="id">The ID of the intention.</param>
        /// <returns>The intention data.</returns>
        public Intention GetIntention(string id)
        {
            return intentionData.GetIntention(id);
        }

        /// <summary>
        /// Retrieves enemy skill data based on the given ID.
        /// </summary>
        /// <param name="id">The ID of the enemy skill.</param>
        /// <returns>The enemy skill data.</returns>
        public EnemySkillData GetEnemySkillInfo(string id)
        {
            return enemySkillData.GetEnemySkillData(id);
        }

        /// <summary>
        /// Retrieves skill data based on the given ID.
        /// </summary>
        /// <param name="id">The ID of the skill.</param>
        /// <returns>The skill data.</returns>
        public SkillInfo GetSkillInfo(string id)
        {
            return skillData.GetSkillInfo(id);
        }
    }
}