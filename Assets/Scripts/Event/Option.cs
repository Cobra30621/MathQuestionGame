using System;
using NueGames.Event.Effect;
using Sirenix.OdinInspector;

namespace NueGames.Event
{
    /// <summary>
    /// 表示事件中的一个选项,包含效果和文本
    /// </summary>
    [Serializable]
    public class Option
    {
        [LabelText("效果")]
        public IEffect Effect;

        [LabelText("選項資料")]
        public OptionData data;
        
        
        public Option(IEffect effect, OptionData optionData)
        {
            Effect = effect;
            data = optionData;
        }
        
        
        /// <summary>
        /// 执行选项的效果
        /// </summary>
        public void ExecuteEffect()
        {
            Effect.Execute();
            EventManager.OnOptionExecuteCompleted(this);
        }
    }
}