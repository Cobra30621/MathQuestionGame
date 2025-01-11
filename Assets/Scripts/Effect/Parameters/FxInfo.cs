using System;
using System.Collections;
using Feedback;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;

namespace Effect.Parameters
{
    /// <summary>
    /// 特效的參數
    /// </summary>
    [Serializable]
    public class FxInfo
    {

        [LabelText("特效物件")]
        [ValueDropdown("GetAssets")]
        [SerializeField] private GameObject fxPrefab;
        
        
        
        public FXPlayer FxPrefab => fxPrefab != null ? fxPrefab.GetComponent<FXPlayer>() : null;
        
        [LabelText("特效產生處")]
        public FxSpawnPosition FxSpawnPosition;

        [LabelText("等待特效的方式")]
        public WaitMethod WaitMethod;
        
        [LabelText("等待時間")]
        [ShowIf("WaitMethod", WaitMethod.WaitDelay)]
        public float Delay = -1;

        
        
        public override string ToString()
        {
            return $"{nameof(fxPrefab)}: {fxPrefab}, {nameof(FxSpawnPosition)}: {FxSpawnPosition}, {nameof(WaitMethod)}: {WaitMethod}, {nameof(Delay)}: {Delay}, {nameof(FxPrefab)}: {FxPrefab}";
        }

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            return AssetGetter.GetAssets(AssetGetter.DataName.Fx);
        }
#endif
    }

    public enum WaitMethod
    {
        None,
        WaitFXFinish,
        WaitDelay
    }
    

}