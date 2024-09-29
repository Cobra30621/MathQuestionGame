using System;
using NueGames.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace NueGames.Event
{
    /// <summary>
    /// 事件画布,用于显示事件和选项
    /// </summary>
    public class EventCanvas : CanvasBase
    {
        [SerializeField] private TextMeshProUGUI eventText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image image;
        
        [SerializeField] private OptionButton[] optionButtons;
        [SerializeField] private OptionButton leaveButton;
        
        private EventManager eventManager => GameManager.EventManager;

        [SerializeField] private Event currentEvent;
        

        private void Awake()
        {
            EventManager.OnLeaveEventSystem.AddListener(CloseCanvas);
            EventManager.OnExecuteCompleted.AddListener(OnOptionExecuteCompleted);
        }


        public override void OpenCanvas()
        {
            base.OpenCanvas();
            ShowEvent();
        }


        /// <summary>
        /// 显示事件
        /// </summary>
        [Button]
        public void ShowEvent()
        {
            currentEvent = eventManager.GetRandomEvent();
            if (currentEvent!= null)
            {
                eventText.text = currentEvent.data.Text;
                nameText.text = currentEvent.data.nameText;
                image.sprite = currentEvent.data.eventSprite;
                
                for (int i = 0; i < optionButtons.Length; i++)
                {
                    if (i < currentEvent.data.OptionData.Count)
                    {
                        optionButtons[i].gameObject.SetActive(true);
                        optionButtons[i].SetUI(currentEvent.Options[i]);
                    }
                    else
                    {
                        optionButtons[i].gameObject.SetActive(false);
                    }
                }
            }
            
            leaveButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 选项执行完成后的回调
        /// </summary>
        /// <param name="option">执行完成的选项</param>
        public void OnOptionExecuteCompleted(Option option)
        {
            eventText.text = option.data.AfterSelectionText;
            // 在这里可以添加更多的逻辑,比如隐藏选项按钮,显示继续按钮等
            foreach (var optionButton in optionButtons)
            {
                optionButton.gameObject.SetActive(false);
            }
            
            leaveButton.gameObject.SetActive(true);
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
            eventText.text = "";
            foreach (var button in optionButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}