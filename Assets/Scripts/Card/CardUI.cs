using NueGames.Data.Collection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.Card
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] protected Image cardImage;
        [SerializeField] protected Image passiveImage;
        [SerializeField] protected TextMeshProUGUI nameTextField;
        [SerializeField] protected TextMeshProUGUI descTextField;
        [SerializeField] protected TextMeshProUGUI manaTextField;

        public void UpdateUI(CardData CardData, int mana)
        {
            nameTextField.text = CardData.CardName;
            descTextField.text = CardData.MyDescription;
            manaTextField.text = CardData.ManaCost.ToString();
            cardImage.sprite = CardData.CardSprite;
            manaTextField.text = mana.ToString();
        }

        public void SetPlayable(bool playable)
        {
            passiveImage.gameObject.SetActive(playable);
        }
    }
}