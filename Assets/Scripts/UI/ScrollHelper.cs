using Sirenix.OdinInspector;

namespace UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class ScrollHelper : MonoBehaviour
    {
        [Required]
        public ScrollRect scrollRect;

        /// <summary>
        /// 將 ScrollRect 立即捲動到頂端，並避免彈跳或滑動效果
        /// </summary>
        public void SnapToTopImmediately()
        {
            scrollRect.verticalNormalizedPosition = 1f;
            
        }
    }

}