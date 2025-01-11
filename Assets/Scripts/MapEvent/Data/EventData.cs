using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MapEvent.Data
{
    /// <summary>
    /// 表示一个具有可选择选项和文本设置的单独事件
    /// </summary>
    [Serializable]
    public class EventData
    {
        [BoxGroup("描述")]
        [LabelText("標題")]
        public string Text;

        [BoxGroup("描述")]
        [LabelText("說話的人")]
        public string nameText;

        [BoxGroup("描述")]
        [LabelText("事件描述")]
        public string Description;

        [BoxGroup("描述")]
        [LabelText("事件圖片")]
        public Sprite eventSprite;
        
        [BoxGroup("選項")]
        [LabelText("選項名稱")]
        public List<OptionData> OptionData;
    }
}