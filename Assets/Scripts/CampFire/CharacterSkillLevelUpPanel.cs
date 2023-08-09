using UnityEngine;

namespace CampFire
{
    public class CharacterSkillLevelUpPanel : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;

        public void Open()
        {
            mainPanel.SetActive(true);
        }

        public void Close()
        {
            mainPanel.SetActive(false);
        }

    }
}