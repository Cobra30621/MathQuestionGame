using NueGames.Action.MathAction;
using NueGames.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.CharacterAbility
{
    public class CharacterAbilityManager : SerializedMonoBehaviour
    {
        #region Instance(Singleton)
        private static CharacterAbilityManager instance;
        
        public static CharacterAbilityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CharacterAbilityManager>();
    
                    if (instance == null)
                    {
                        Debug.LogError($"The GameObject with {typeof(CharacterAbilityManager)} does not exist in the scene, " +
                                       $"yet its method is being called.\n" +
                                       $"Please add {typeof(CharacterAbilityManager)} to the scene.");
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
    
        public CharacterAbility CharacterAbility;
        public QuestionSetting QuestionSetting;
        
        [ContextMenu("Play Ability")]
        public void PlayAbility()
        {
            GameActionExecutor.AddToBottom(new EnterMathQuestioningAction(
                CharacterAbility.QuestionActionParameters, QuestionSetting));
        }
    }
}