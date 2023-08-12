using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.CharacterAbility
{
    public class CharacterSkillUI : SerializedMonoBehaviour
    {
        [SerializeField] private GameObject infoPanel;
        
        [SerializeField] private Text abilityName, description, skillCount;
        [SerializeField] private Button playSkillButton;


        private void Awake()
        {
            CharacterSkillManager.OnSkillCountChange += UpdateSkillInfo;
            Debug.Log("subscribe OnSkillCountChange");
        }


        public void ToggleInfoPanel()
        {
            SetInfoPanelActive(!infoPanel.activeSelf);
        }

        public void SetInfoPanelActive(bool active)
        {
            infoPanel.SetActive(active);
            abilityName.text = CharacterSkillManager.Instance.CharacterSkill.skillName;
            description.text = CharacterSkillManager.Instance.CharacterSkill.skillDescription;
        }

        private void UpdateSkillInfo(int count)
        {
            skillCount.text = $"剩餘次數：{count}";
            playSkillButton.interactable = CharacterSkillManager.Instance.EnablePlaySkill();
        }

        public void PlayCharacterAbility()
        {
            CharacterSkillManager.Instance.PlaySkill();
        }
    }
}