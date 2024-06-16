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

        public void UpdateUI(CardInfo cardInfo)
        {
            nameTextField.text = cardInfo.CardData.CardName + $" {cardInfo.Level}";
            descTextField.text = cardInfo.Description;
            manaTextField.text = cardInfo.ManaCost.ToString();
            cardImage.sprite = cardInfo.CardData.CardSprite;
        }
        
        public void SetPlayable(bool playable)
        {
            passiveImage.gameObject.SetActive(playable);
        }
    }
}