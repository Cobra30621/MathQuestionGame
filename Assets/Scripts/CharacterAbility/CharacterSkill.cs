using NueGames.Action.MathAction;
using Question.QuestionAction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.CharacterAbility
{
    [CreateAssetMenu(fileName = "Character Skill", menuName = "NueDeck/CharacterSkill", order = 0)]
    public class CharacterSkill : SerializedScriptableObject
    {
        /// <summary>
        /// 技能名稱
        /// </summary>
        public string skillName;
        /// <summary>
        /// 技能描述
        /// </summary>
        public string skillDescription;
        /// <summary>
        /// 技能使用次數
        /// </summary>
        public int skillCount;
        /// <summary>
        /// 數學行為
        /// </summary>
        public QuestionActionBase QuestionAction;
    }
}