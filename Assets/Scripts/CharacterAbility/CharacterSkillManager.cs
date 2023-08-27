using System;
using NueGames.Action.MathAction;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.CharacterAbility
{
    /// <summary>
    /// 角色能力管理器
    /// </summary>
    public class CharacterSkillManager : Singleton<CharacterSkillManager>
    {


        #region Event
        /// <summary>
        /// 當角色使用能力時
        /// </summary>
        public static System.Action OnPlaySkill;
        /// <summary>
        /// 當技能使用次數改變時
        /// </summary>
        public static Action<int> OnSkillCountChange ;
        

        #endregion

        #region 參數


        public CharacterSkill CharacterSkill => _characterSkill;
        [SerializeField] private CharacterSkill _characterSkill;
        
        public int SkillCount => skillCount;
        private int skillCount;
        

        #endregion
        

        public void SetCharacterSkill(CharacterSkill characterSkill)
        {
            Debug.Log($"Set Skill {characterSkill}");
            _characterSkill = characterSkill;
            SetSkillCount(characterSkill.skillCount);
        }

        
        
        [ContextMenu("Play Skill")]
        public void PlaySkill()
        {
            if (EnablePlaySkill())
            {
                AddSkillCount(-1);
                GameActionExecutor.AddToBottom(new EnterMathQuestioningAction(
                    _characterSkill.QuestionAction));
                OnPlaySkill?.Invoke();
            }
        }

        #region Skill Count

        public bool EnablePlaySkill()
        {
            return skillCount > 0;
        }

        /// <summary>
        /// 設定能力使用次數
        /// </summary>
        /// <param name="count"></param>
        public void SetSkillCount(int count)
        {
            skillCount = count;
            OnSkillCountChange?.Invoke(skillCount);
        }
        
        /// <summary>
        /// 增加能力使用次數
        /// </summary>
        /// <param name="addCount"></param>
        public void AddSkillCount(int addCount)
        {
            skillCount += addCount;
            OnSkillCountChange?.Invoke(skillCount);
        }

        #endregion
    }
}