using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Event
{
    /// <summary>
    /// 表示一个具有可选择选项和文本设置的单独事件
    /// </summary>
    [Serializable]
    public class EventData
    {
        
        [LabelText("標題")]
        public string Text;

        [LabelText("說話的人")]
        public string nameText;

        [LabelText("事件描述")]
        public string Description;

        [LabelText("事件圖片")]
        public Sprite eventSprite;
        
        [LabelText("選項名稱")]
        public List<OptionData> OptionData;
    }
}