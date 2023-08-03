using System;
using NueGames.Action.MathAction;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.CharacterAbility
{
    public class CharacterSkillManager : SerializedMonoBehaviour
    {
        #region Instance(Singleton)
        private static CharacterSkillManager instance;
        
        public static CharacterSkillManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CharacterSkillManager>();
    
                    if (instance == null)
                    {
                        Debug.LogError($"The GameObject with {typeof(CharacterSkillManager)} does not exist in the scene, " +
                                       $"yet its method is being called.\n" +
                                       $"Please add {typeof(CharacterSkillManager)} to the scene.");
                    }
                }
    
                return instance;
            }
        }
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        
        #endregion


        #region Event

        public static System.Action OnPlaySkill;

        public static Action<int> OnSkillCountChange;
        

        #endregion
        
        
        
        public QuestionSetting QuestionSetting;

        public CharacterSkill CharacterSkill => _characterSkill;
        [SerializeField] private CharacterSkill _characterSkill;
        
        public int SkillCount => skillCount;
        private int skillCount;
        
        

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
                ChangeSkillCount(-1);
                GameActionExecutor.AddToBottom(new EnterMathQuestioningAction(
                    _characterSkill.QuestionActionParameters, QuestionSetting));
                
            }
            
            
        }

        #region Skill Count

        public bool EnablePlaySkill()
        {
            return skillCount > 0;
        }

        public void SetSkillCount(int count)
        {
            skillCount = count;
            OnSkillCountChange?.Invoke(skillCount);
        }
        
        public void ChangeSkillCount(int addCount)
        {
            skillCount += addCount;
            OnSkillCountChange?.Invoke(skillCount);
        }

        #endregion
    }
}