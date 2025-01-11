using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;

namespace Characters.Display
{
    /// <summary>
    /// 在卡牌, 敵人行動設計，指定播放的角色 Feedback
    /// </summary>
    public class CustomerFeedbackSetting
    {
        [InfoBox("請前往 Assets/Prefabs/Characters，加入想播放客製化 Feedback 的角色")]
        [SerializeField] private CharacterBase targetCharacter;
        
        [ShowIf("ShowCustomFeedbackKey")]
        [ValueDropdown("GetFeedbackKeys")]
        public string customFeedbackKey;
        
        private List<string> GetFeedbackKeys()
        {
            var keys = targetCharacter?.GetCustomFeedbackList();
            
            if (!(keys?.Count > 0))
            {
                customFeedbackKey = "";
            }
            return keys;
        }


        private bool ShowCustomFeedbackKey()
        {
            return (targetCharacter != null);
        }

        
#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            var asset = AssetGetter.GetAssets(AssetGetter.DataName.Character);
            
            Debug.Log($"asset {asset}");
            // Debug.Log($"asset {(List<CharacterBase>)asset}");
            return asset;
        }
#endif
    }
}