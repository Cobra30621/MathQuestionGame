using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Enums;
using NueGames.UI;
using UnityEngine;

namespace NueGames.Data.Containers
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

        public PowerData GetPowerData(PowerType powerType)
        {
            return PowerList.FirstOrDefault(x => x.PowerType == powerType);
        }
    }

    /// <summary>
    /// 能力資料
    /// </summary>
    [Serializable]
    public class PowerData
    {
        
        [SerializeField] private string titleText;
        
        [SerializeField][TextArea] private string contentText;
        
        [SerializeField] private PowerType powerType;
        
        [SerializeField] private Sprite iconSprite;
        
        /// <summary>
        /// 能力類型
        /// </summary>
        public PowerType PowerType => powerType;
        /// <summary>
        /// 能力 Icon
        /// </summary>
        public Sprite IconSprite => iconSprite;
        
        /// <summary>
        /// 能力名稱
        /// </summary>
        public string GetHeader(string overrideKeywordHeader = "")
        {
            if(titleText != "")
                return titleText;
            return string.IsNullOrEmpty(overrideKeywordHeader) ? powerType.ToString() : overrideKeywordHeader;
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