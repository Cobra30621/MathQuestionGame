using System;
using Sirenix.OdinInspector;

namespace MapEvent.Data
{
    [Serializable]
    public class OptionData
    {
        [LabelText("效果資料")]
        public EffectData EffectData;
        
        [LabelText("選項名稱")]
        public string OptionText;
        
        [LabelText("選擇後的描述")]
        public string AfterSelectionText;
    }
}