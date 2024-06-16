using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card.Display
{
    public class UICard : CardBase,IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler,IPointerUpHandler
    {
        [SerializeField] private float showScaleRate = 1.15f;
 
        private Vector3 _initalScale;
        
        public System.Action OnCardChose;

        private void Awake()
        {
            _initalScale = transform.localScale;
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            transform.localScale = _initalScale * showScaleRate;
        }



        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            transform.localScale = _initalScale;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            OnCardChose?.Invoke();
            
        }
    }
}