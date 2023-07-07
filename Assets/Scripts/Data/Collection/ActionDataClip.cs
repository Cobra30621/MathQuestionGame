using System.Collections.Generic;
using NueGames.Action.MathAction;
using UnityEngine;

namespace NueGames.Data.Collection
{
    /// <summary>
    /// 遊戲行為所需資料
    /// 為了避免迴圈產生
    /// </summary>
    [SerializeField]
    public class ActionDataClip
    {
        public ActionClipType ActionClipType;
        public List<ActionData> ActionList;
        public List<ActionData> TriggerActionList;
        public MathQuestioningActionParameters MathQuestioningActionParameters;
    }


    public enum ActionClipType
    {
        Normal,
        Math,
        Random
    }
}