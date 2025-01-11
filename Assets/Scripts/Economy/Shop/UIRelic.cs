using Relic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Economy.Shop
{
    public class UIRelic : MonoBehaviour, 
        IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private float showScaleRate = 1.15f;
        public System.Action OnRelicChose ;
        
        [Required]
        [SerializeField] protected Image iconImage;
        [Required]
        [SerializeField] protected TextMeshProUGUI nameTextField;
        [Required]
        [SerializeField] protected TextMeshProUGUI descTextField;
        [Required]
        [SerializeField] protected Button selectedButton;
        
        private Vector3 _initalScale;
        
        private void Awake()
        {
            _initalScale = transform.localScale;
            selectedButton.onClick.AddListener((() => OnRelicChose?.Invoke()));
        }

        public void Init(RelicInfo relicInfo)
        {
            iconImage.sprite = relicInfo.data.IconSprite;
            nameTextField.text = relicInfo.data.Title;
            descTextField.text = relicInfo.GetDescription();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _initalScale * showScaleRate;
        }



        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _initalScale;
        }
        

    }
}