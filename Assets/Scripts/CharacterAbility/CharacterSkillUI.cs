using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.CharacterAbility
{
    public class CharacterSkillUI : SerializedMonoBehaviour
    {
        [SerializeField] private Text abilityName, description, skillCount;
        [SerializeField] private Button playSkillButton;


        private void OnEnable()
        {
            CharacterSkillManager.OnSkillCountChange += UpdateSkillInfo;
        }

        private void OnDisable()
        {
            CharacterSkillManager.OnSkillCountChange -= UpdateSkillInfo;
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            abilityName.text = CharacterSkillManager.Instance.CharacterSkill.skillName;
            description.text = CharacterSkillManager.Instance.CharacterSkill.skillDescription;
            playSkillButton.interactable = CharacterSkillManager.Instance.EnablePlaySkill();
        }

        private void UpdateSkillInfo(int count)
        {
            skillCount.text = $"剩餘次數：{count}";
        }

        public void PlayCharacterAbility()
        {
            CharacterSkillManager.Instance.PlaySkill();
        }
    }
}