using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace NueGames.Event
{
    /// <summary>
    /// 表示事件选项的按钮
    /// </summary>
    public class OptionButton : MonoBehaviour
    {
        [Required]
        [SerializeField] private Button button;
        [Required]
        [SerializeField] private TextMeshProUGUI optionText;
        
        [ShowInInspector]
        private Option currentOption;


        private void Awake()
        {
            button.onClick.AddListener(OnSelect);
        }


        /// <summary>
        /// 设置按钮UI
        /// </summary>
        /// <param name="option">要显示的选项</param>
        public void SetUI(Option option)
        {
            currentOption = option;
            optionText.text = option.OptionText;
        }

        /// <summary>
        /// 当按钮被选中时执行
        /// </summary>
        public void OnSelect()
        {
            if (currentOption != null)
            {
                currentOption.ExecuteEffect();
            }
        }
    }
}