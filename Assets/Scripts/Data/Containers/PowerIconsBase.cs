using NueGames.Data.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.UI
{
    public class PowerIconsBase : MonoBehaviour
    {
        [SerializeField] private Image statusImage;
        [SerializeField] private TextMeshProUGUI statusValueText;

        public PowerData MyPowerData { get; private set; } = null;

        public Image StatusImage => statusImage;

        public TextMeshProUGUI StatusValueText => statusValueText;

        public void SetStatus(PowerData powerData)
        {
            MyPowerData = powerData;
            StatusImage.sprite = powerData.IconSprite;
            
            StatusValueText.gameObject.SetActive(!powerData.HideAmount);
        }

        public void SetStatusValue(int statusValue)
        {
            StatusValueText.text = statusValue.ToString();
        }
    }
}