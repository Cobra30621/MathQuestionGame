using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Power
{
    /// <summary>
    /// 所有能力的資料
    /// </summary>
    [CreateAssetMenu(fileName = "Powers", menuName = "NueDeck/Containers/Powers", order = 2)]
    public class PowersData : ScriptableObject
    {
        [SerializeField] private PowerIconsBase powerBasePrefab;
        [SerializeField] private List<PowerData> powerList;

        
        /// <summary>
        /// 能力 Icon 的遊戲物件(Prefab)
        /// </summary>
        public PowerIconsBase PowerBasePrefab => powerBasePrefab;
        /// <summary>
        /// 能力清單
        /// </summary>
        public List<PowerData> PowerList => powerList;

        public PowerData GetPowerData(PowerName targetPower)
        {
            var targetData = PowerList.FirstOrDefault(x => x.PowerName == targetPower);
            if (targetData == null)
            {
                Debug.LogError($"找不到 Power {targetPower} 的 powerData" +
                               $"請去 Assets/NueGames/NueDeck/Data/Containers/Powers.asset設定");
            }
            
            
            return targetData;
        }

        
        
        /// <summary>
        /// 將 powerList 依照 PowerName 進行排序
        /// </summary>
        [Button]
        public void SortPowerList()
        {
            powerList = powerList.OrderBy(x => x.PowerName).ToList();
        }

    }

    /// <summary>
    /// 能力資料
    /// </summary>
    [Serializable]
    public class PowerData
    {
        [SerializeField] private PowerName powerName;
        
        [SerializeField] private string titleText;
        
        [SerializeField][TextArea] private string contentText;
        
        
        
        [SerializeField] private Sprite iconSprite;

        [LabelText("隱藏層數")]
        [SerializeField] private bool hideAmount = false;
        
        /// <summary>
        /// 能力類型
        /// </summary>
        public PowerName PowerName => powerName;
        /// <summary>
        /// 能力 Icon
        /// </summary>
        public Sprite IconSprite => iconSprite;

        public bool HideAmount => hideAmount;
        
        /// <summary>
        /// 能力名稱
        /// </summary>
        public string GetHeader(string overrideKeywordHeader = "")
        {
            if(titleText != "")
                return titleText;
            return string.IsNullOrEmpty(overrideKeywordHeader) ? powerName.ToString() : overrideKeywordHeader;
        }

        /// <summary>
        /// 能力說明
        /// </summary>
        public string GetContent(string overrideContent = "")
        {
            return string.IsNullOrEmpty(overrideContent) ? contentText : overrideContent;
        }
    }
}