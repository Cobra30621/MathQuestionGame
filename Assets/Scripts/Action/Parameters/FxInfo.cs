using NueGames.Enums;
using Sirenix.OdinInspector;

namespace Action.Parameters
{
    /// <summary>
    /// 特效的參數
    /// </summary>
    public class FxInfo
    {
        [FoldoutGroup("播放的特效")]
        [PropertyTooltip("特效名稱")]
        public FxName FxName;
        
        [FoldoutGroup("播放的特效")]
        [PropertyTooltip("特效產生處")]
        public FxSpawnPosition FxSpawnPosition;

        public FxInfo(FxName fxName, FxSpawnPosition fxSpawnPosition)
        {
            FxName = fxName;
            FxSpawnPosition = fxSpawnPosition;
        }
    }
}