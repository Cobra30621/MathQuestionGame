using NueGames.Data.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.UI
{
    public class RelicIconsBase : MonoBehaviour
    {
        [SerializeField] private Image statusImage;
        [SerializeField] private TextMeshProUGUI statusValueText;

        public RelicData MyRelicData { get; private set; } = null;

        public Image StatusImage => statusImage;

        public TextMeshProUGUI StatusValueText => statusValueText;

        public void SetData(RelicData relicData)
        {
            MyRelicData = relicData;
            StatusImage.sprite = relicData.IconSprite;
            
        }

        public void SetValue(int statusValue)
        {
            StatusValueText.text = statusValue.ToString();
        }
    }
}