using System;
using MapEvent.Data;
using MapEvent.Effect;
using Sirenix.OdinInspector;

namespace MapEvent
{
    /// <summary>
    /// 表示事件中的一个选项,包含效果和文本
    /// </summary>
    [Serializable]
    public class Option
    {
        [LabelText("效果")]
        public IEventEffect EventEffect;

        [LabelText("選項資料")]
        public OptionData data;
        
        
        public Option(IEventEffect eventEffect, OptionData optionData)
        {
            EventEffect = eventEffect;
            data = optionData;
        }
        
        
        /// <summary>
        /// 执行选项的效果
        /// </summary>
        public void ExecuteEffect()
        {
            EventEffect.Execute(() =>
            {
                EventManager.OnOptionExecuteCompleted(this);
            });
            
        }
    }
}