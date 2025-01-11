using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Economy
{
    public class TopBar : MonoBehaviour
    {
        [Required]
        [SerializeField] private TextMeshProUGUI moneyText, stoneText;

        private void Update()
        {
            moneyText.text = CoinManager.Instance.Money.ToString();
            stoneText.text = CoinManager.Instance.Stone.ToString();
        }
    }
}