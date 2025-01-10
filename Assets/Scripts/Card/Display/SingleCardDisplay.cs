using Card.Data;
using NueGames.Data.Collection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.Card
{
    public class SingleCardDisplay : MonoBehaviour
    {
        [SerializeField] protected Image cardImage;
        [SerializeField] protected Image passiveImage;
        [SerializeField] protected TextMeshProUGUI nameTextField;
        [SerializeField] protected TextMeshProUGUI descTextField;
        [SerializeField] protected TextMeshProUGUI manaTextField;

        [SerializeField] protected Image backgroundImage;
        
        public void UpdateUI(CardInfo cardInfo)
        {
            nameTextField.text = cardInfo.CardLevelInfo.TitleLang + $" {cardInfo.Level+1}";
            descTextField.text = cardInfo.CardLevelInfo.DesLang;
            manaTextField.text = cardInfo.ManaCost.ToString();
            cardImage.sprite = cardInfo.CardData.CardSprite;
            backgroundImage.sprite = cardInfo.CardSprite.backgroundSprite;
        }
        
        public void SetPlayable(bool playable)
        {
            passiveImage.gameObject.SetActive(playable);
        }
    }
}