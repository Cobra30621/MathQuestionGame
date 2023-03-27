using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

namespace Kalkatos.DottedArrow
{
    public class Circle : MonoBehaviour
    {
        [SerializeField] private Image outline, center;

        public void SetColor(ArrowColor arrowColor)
        {
            outline.color = arrowColor.OutlineColor;
            center.color = arrowColor.CenterColor;
        }
    }
    
    [Serializable]
    public class ArrowColor
    {
        public Color OutlineColor;
        public Color CenterColor;
    }
}
