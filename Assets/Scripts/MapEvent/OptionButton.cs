using Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MapEvent
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

        [SerializeField][LabelText("是離開按鈕")]
        private bool isLeaveButton;


        private void Awake()
        {
            button.onClick.AddListener(OnSelect);

            if (isLeaveButton)
            {
                currentOption = EventManager.Instance.GetLeaveOption();
            }
        }


        /// <summary>
        /// 设置按钮UI
        /// </summary>
        /// <param name="option">要显示的选项</param>
        public void SetUI(Option option)
        {
            currentOption = option;
            optionText.text = option.data.OptionText;

            button.interactable = option.Effect.IsSelectable();
        }

        /// <summary>
        /// 当按钮被选中时执行
        /// </summary>
        public void OnSelect()
        {
            UIManager.Instance.EventCanvas.OnOptionButtonClick();
            
            if (currentOption != null)
            {
                currentOption.ExecuteEffect();
            }
        }
    }
}