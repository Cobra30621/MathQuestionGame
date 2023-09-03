using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Action.Parameters
{
    /// <summary>
    /// 特效的參數
    /// </summary>
    public class FxInfo
    {

        [FoldoutGroup("播放的特效")]
        [PropertyTooltip("特效物件")]
        public GameObject FxGo;
        
        [FoldoutGroup("播放的特效")]
        [PropertyTooltip("特效產生處")]
        public FxSpawnPosition FxSpawnPosition;

        public FxInfo(GameObject fxGo, FxSpawnPosition fxSpawnPosition)
        {
            FxGo = fxGo;
            FxSpawnPosition = fxSpawnPosition;
        }
    }
}