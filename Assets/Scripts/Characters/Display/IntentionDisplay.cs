using Characters.Enemy;
using Characters.Enemy.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Display
{
    public class IntentionDisplay : MonoBehaviour
    {
        [SerializeField] private Image intentImage;
        [SerializeField] private TextMeshProUGUI nextActionValueText;


        public void Show(EnemySkill currentSkill)
        {
            if (currentSkill == null)
            {
                return;
            }
            
            gameObject.SetActive(true);
            if (currentSkill.GetIntentionValue(out string info))
            {
                ShowWithValue(currentSkill._intention, info);
            }
            else
            {
                ShowWithoutValue(currentSkill._intention);
            }
        }

        private void ShowWithValue(Intention intention, string value)
        {
            nextActionValueText.gameObject.SetActive(true);
            nextActionValueText.text = value;
            intentImage.sprite = intention.IntentionSprite;
        }

        private void ShowWithoutValue(Intention intention)
        {
            nextActionValueText.gameObject.SetActive(false);
            intentImage.sprite = intention.IntentionSprite;
        }
        
    }
}